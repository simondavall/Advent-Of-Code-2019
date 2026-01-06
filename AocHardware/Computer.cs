using System.Diagnostics;

namespace AocHardware;

public class Computer
{
  private long _ip;
  private long _relativeBaseOffset;
  private const int MAX_RAM = 10_000;
  private readonly long[] _ram = new long[MAX_RAM];
  private bool _isHalted = false;
  private bool _isAwaitingInput = false;
  private ParamMode _inputMode;
  private readonly List<long> _output = [];
  private long _cycles = 0;

  private enum ParamMode
  {
    Position, // Value is at position supplied (e.g.) x = _ram[y];
    Immediate, // Use the value supplied (e.g.) x = y;
    Relative, // Value is at poistion supplied + relative base. (e.g.) x = _ram[y] + relateBase 
  }

  private Computer(long[] ram, long ip, long relativeBaseOffset, bool isAwaitingInput, ParamMode inputMode)
  {
    Buffer.BlockCopy(ram, 0, _ram, 0, ram.Length * sizeof(long));
    _ip = ip;
    _relativeBaseOffset = relativeBaseOffset;
    _isAwaitingInput = isAwaitingInput;
    _inputMode = inputMode;
  }

  public Computer(long[] program)
  {
    Debug.Assert(program.Length < MAX_RAM, $"Out Of Memory. Program is too large, get some more RAM. Size:{program.Length}");
    _ip = 0;
    _relativeBaseOffset = 0;
    Buffer.BlockCopy(program, 0, _ram, 0, program.Length * sizeof(long));
  }

  public Computer Clone()
  {
    return new Computer(_ram, _ip, _relativeBaseOffset, _isAwaitingInput, _inputMode);
  }

  public long ReadMemory(long address)
  {
    if (address < 0 || address >= _ram.Length)
      throw new OutOfMemoryException($"Invalid memory address. Address expected between 0 and {_ram.Length - 1}. Found:{address}");

    return _ram[address];
  }

  public void SetMemory(long address, long value)
  {
    if (address < 0 || address >= _ram.Length)
      throw new OutOfMemoryException($"Invalid memory address. Address expected between 0 and {_ram.Length - 1}. Found:{address}");

    _ram[address] = value;
  }

  public void SetInput(long value)
  {
    if (!_isAwaitingInput)
      throw new ApplicationException("Attempt to set input when the progam is not awaiting input.");

    Input(value);
    _isAwaitingInput = false;
  }

  public long[] GetOutput()
  {
    long[] output = _output.ToArray();
    _output.Clear();
    return output;
  }

  public long Cycles => _cycles;

  public bool IsHalted => _isHalted;

  public bool IsAwaitingInput => _isAwaitingInput;

  public void Execute()
  {
    while (!_isAwaitingInput && !_isHalted) {
      Debug.Assert(_ip < _ram.Length && _ip >= 0, $"Instruction pointer is out of bounds. Terminaling program. Ip:{_ip}");

      var (opCode, modes) = GetNextOpCode();
      _cycles++;
      switch (opCode) {
        case 1: Add(modes); break;
        case 2: Multiply(modes); break;
        case 3: SetAwaitingInput(modes); break;
        case 4: SetOutput(modes); break;
        case 5: JumpIfTrue(modes); break;
        case 6: JumpIfFalse(modes); break;
        case 7: LessThan(modes); break;
        case 8: Equals(modes); break;
        case 9: AdjustRelativeBaseOffset(modes); break;
        case 99: SetIsHalted(); break;
        default:
          throw new InvalidOperationException($"Unrecognized opCode: '{opCode}'");
      }
    }
  }

  private (int, ParamMode[]) GetNextOpCode()
  {
    ParamMode[] modes = new ParamMode[3];
    var instruction = (int)_ram[_ip];
    var opcode = instruction % 100;
    instruction /= 100;
    int index = 0;
    while (instruction > 0) {
      modes[index++] = (ParamMode)(instruction % 10);
      instruction /= 10;
    }
    return (opcode, modes);
  }

  private void Add(ParamMode[] modes)
  {
    long a = GetValue(modes[0], _ip + 1);
    long b = GetValue(modes[1], _ip + 2);
    long addr = GetWriteAddress(modes[2], _ip + 3);
    _ram[addr] = a + b;
    _ip += 4;
  }

  private void Multiply(ParamMode[] modes)
  {
    long a = GetValue(modes[0], _ip + 1);
    long b = GetValue(modes[1], _ip + 2);
    long addr = GetWriteAddress(modes[2], _ip + 3);
    _ram[addr] = a * b;
    _ip += 4;
  }

  private void SetAwaitingInput(ParamMode[] modes)
  {
    _isAwaitingInput = true;
    _inputMode = modes[0];
  }

  private void Input(long input)
  {
    long addr = GetWriteAddress(_inputMode, _ip + 1);
    _ram[addr] = input;
    _ip += 2;
  }

  private void SetOutput(ParamMode[] modes)
  {
    long a = GetValue(modes[0], _ip + 1);
    _output.Add(a);
    _ip += 2;
  }

  private void JumpIfTrue(ParamMode[] modes)
  {
    long a = GetValue(modes[0], _ip + 1);
    long b = GetValue(modes[1], _ip + 2);
    if (a > 0)
      _ip = b;
    else
      _ip += 3;
  }

  private void JumpIfFalse(ParamMode[] modes)
  {
    long a = GetValue(modes[0], _ip + 1);
    long b = GetValue(modes[1], _ip + 2);
    if (a == 0)
      _ip = b;
    else
      _ip += 3;
  }

  private void LessThan(ParamMode[] modes)
  {
    long a = GetValue(modes[0], _ip + 1);
    long b = GetValue(modes[1], _ip + 2);
    long addr = GetWriteAddress(modes[2], _ip + 3);
    _ram[addr] = a < b ? 1 : 0;
    _ip += 4;
  }

  private void Equals(ParamMode[] modes)
  {
    long a = GetValue(modes[0], _ip + 1);
    long b = GetValue(modes[1], _ip + 2);
    long addr = GetWriteAddress(modes[2], _ip + 3);
    _ram[addr] = a == b ? 1 : 0;
    _ip += 4;
  }

  private void AdjustRelativeBaseOffset(ParamMode[] modes)
  {
    long a = GetValue(modes[0], _ip + 1);
    _relativeBaseOffset += a;
    _ip += 2;
  }

  private void SetIsHalted()
  {
    _isHalted = true;
  }

  private long GetValue(ParamMode mode, long ip)
  {
    return mode switch {
      ParamMode.Position => _ram[_ram[ip]],
      ParamMode.Immediate => _ram[ip],
      ParamMode.Relative => _ram[_ram[ip] + _relativeBaseOffset],
      _ => throw new ApplicationException($"Unknown parameter mode. Value:'{mode}'")
    };
  }

  private long GetWriteAddress(ParamMode mode, long ip)
  {
    return mode switch {
      ParamMode.Position => _ram[ip],
      ParamMode.Immediate => _ram[ip],
      ParamMode.Relative => _ram[ip] + _relativeBaseOffset,
      _ => throw new ApplicationException($"Unknown parameter mode. Value:'{mode}'")
    };
  }
}
