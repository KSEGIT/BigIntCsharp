# INT96 Timestamp Parser for C#

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A lightweight, efficient C# library for parsing INT96 timestamps into .NET `DateTime` objects. This implementation is specifically designed for handling Apache Parquet INT96 timestamp format with proper endianness handling and validation.

## 🎯 Overview

INT96 timestamps are a 12-byte format commonly used in Apache Parquet files to represent timestamps with nanosecond precision. This library provides robust parsing utilities to convert these timestamps into standard .NET `DateTime` objects, making it easier to work with Parquet data in C# applications.

## ✨ Key Features

- **Multiple Parsing Methods**: Supports both string-based hex parsing and `BigInteger`-based parsing
- **Proper Endianness Handling**: Correctly handles byte order conversion for accurate timestamp representation
- **Input Validation**: Validates Julian day ranges and input format to prevent errors
- **Nanosecond Precision**: Handles nanosecond-level timestamp precision conversion
- **Zero Dependencies**: Uses only .NET standard libraries (`System.Numerics` for BigInteger support)
- **Well-Documented**: Clear inline documentation and comprehensive examples

## 📦 Installation

Simply copy the `BigIntCsharp.cs` file into your C# project. The library requires:

- .NET Framework 4.0+ or .NET Core 1.0+ (for `System.Numerics.BigInteger` support)
- C# 7.0 or later

### Manual Installation

```bash
# Clone the repository
git clone https://github.com/KSEGIT/BigIntCsharp.git

# Copy the file to your project
cp BigIntCsharp/BigIntCsharp.cs YourProject/
```

## 🚀 Usage

### Basic Usage with Hex String

```csharp
using System;

var parser = new Int96TimestampParser();

// Parse an INT96 timestamp from hex string format
string int96Hex = "0x8096980000000000E8030000";
DateTime result = parser.ParseInt96Timestamp(int96Hex);

Console.WriteLine($"Parsed DateTime: {result:yyyy-MM-dd HH:mm:ss.ffffff}");
```

### Using BigInteger

```csharp
using System;
using System.Numerics;
using System.Globalization;

var parser = new Int96TimestampParser();

// Parse from BigInteger
BigInteger int96Value = BigInteger.Parse("00000003E800000000000000980680", NumberStyles.HexNumber);
DateTime result = parser.DateFromInt96Timestamp(int96Value);

Console.WriteLine($"Parsed DateTime: {result:yyyy-MM-dd HH:mm:ss.fff}");
```

### Static Method for Quick Conversion

```csharp
using System;
using System.Numerics;
using System.Globalization;

// Use the static method for one-off conversions
BigInteger int96Value = BigInteger.Parse("00000003E800000000000000980680", NumberStyles.HexNumber);
DateTime result = Int96TimestampParser.DateFromInt96Timestamp2(int96Value);

Console.WriteLine($"Parsed DateTime: {result:yyyy-MM-dd HH:mm:ss.fff}");
```

## 📚 API Reference

### `ParseInt96Timestamp(string int96TimestampString)`

Parses an INT96 timestamp from a hexadecimal string.

**Parameters:**
- `int96TimestampString` (string): A 26-character hex string (including "0x" prefix) representing 12 bytes

**Returns:** `DateTime` - The parsed timestamp in UTC

**Exceptions:**
- `ArgumentException`: If the input string is not 26 characters long
- `ArgumentOutOfRangeException`: If the Julian day is outside valid range (1970-01-01 to 9999-12-31)

### `DateFromInt96Timestamp(BigInteger int96Timestamp)`

Converts an INT96 timestamp from BigInteger format to DateTime.

**Parameters:**
- `int96Timestamp` (BigInteger): The INT96 timestamp as a BigInteger value

**Returns:** `DateTime` - The parsed timestamp

### `DateFromInt96Timestamp2(BigInteger int96Timestamp)` (Static)

Static version of the BigInteger conversion method with optimized casting.

**Parameters:**
- `int96Timestamp` (BigInteger): The INT96 timestamp as a BigInteger value

**Returns:** `DateTime` - The parsed timestamp

## 🎓 Understanding INT96 Timestamps

INT96 timestamps consist of:
- **First 8 bytes**: Nanoseconds within the day (as a 64-bit integer)
- **Last 4 bytes**: Julian day number (as a 32-bit integer)

The Julian day is the number of days since January 1, 4713 BCE. The Unix epoch (January 1, 1970) corresponds to Julian day 2,440,588.

## 💡 Why Use This Implementation?

### 1. **Optimized for Parquet Data Processing**
If you're working with Apache Parquet files in C# (using libraries like Parquet.NET or similar), you'll frequently encounter INT96 timestamps. This library provides a battle-tested, reliable way to parse them.

### 2. **Handles Edge Cases**
- Validates Julian day ranges to prevent invalid dates
- Properly handles endianness conversions
- Supports multiple input formats (hex strings and BigInteger)

### 3. **Performance Considerations**
- Minimal allocations
- Efficient bit manipulation operations
- No external dependencies to slow down your application

### 4. **Production-Ready**
- Comprehensive input validation
- Clear error messages for troubleshooting
- Well-commented code for maintainability

### 5. **Flexibility**
Three different parsing methods let you choose the approach that best fits your data source:
- `ParseInt96Timestamp`: For hex string inputs
- `DateFromInt96Timestamp`: For BigInteger instances
- `DateFromInt96Timestamp2`: Static method for one-off conversions

## 🔍 Common Use Cases

- **Data Engineering**: Converting Parquet file timestamps in ETL pipelines
- **Data Analytics**: Reading historical data from Parquet files
- **Database Migration**: Migrating data from Parquet to SQL databases
- **Big Data Processing**: Working with data exported from Spark, Hive, or Impala

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Daniel Kiska** (DanyITnerd)

## 🙏 Acknowledgments

- Original implementation inspired by various Stack Overflow discussions on INT96 timestamp parsing
- Apache Parquet format specification for timestamp encoding details

---

**Note**: This library focuses specifically on parsing INT96 timestamps. For comprehensive Parquet file handling, consider using it alongside [Parquet.NET](https://github.com/aloneguid/parquet-dotnet) or similar libraries.
