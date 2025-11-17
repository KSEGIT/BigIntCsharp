// Author: Daniel Kiska
// Description: INT96 timestamp parsing utilities with validation and proper endianness handling.

using System;
using System.Globalization;
using System.Numerics;

public class Int96TimestampParser
{
    public DateTime ParseInt96Timestamp(string int96TimestampString)
    {
        // Ensure the input string is of the correct length
        if (int96TimestampString.Length != 26) // 24 characters + 2 for the hex prefix
        {
            throw new ArgumentException("The INT96 timestamp string must be 26 characters long (including the hex prefix), representing 12 bytes.", nameof(int96TimestampString));
        }

        // Remove the hex prefix ("0x") if present
        if (int96TimestampString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            int96TimestampString = int96TimestampString.Substring(2);
        }

        // Parse the INT96 timestamp
        byte[] int96Bytes = new byte[12];
        for (int i = 0; i < 12; i++)
        {
            int96Bytes[i] = byte.Parse(int96TimestampString.Substring(i * 2, 2), NumberStyles.HexNumber);
        }

        // Handle endianness for conversion
        Array.Reverse(int96Bytes, 0, 8); // Reversing first 8 bytes for proper conversion

        // Get the nanoseconds within the day and the Julian day
        long nanosecondsWithinDay = BitConverter.ToInt64(int96Bytes, 0);
        int julianDay = BitConverter.ToInt32(int96Bytes, 8);

        // Ensure the Julian day is within the valid range
        if (julianDay < 2440588 || julianDay > 5373484) // January 1, 1970 to January 1, 10000
        {
            throw new ArgumentOutOfRangeException(nameof(int96TimestampString), "The Julian day is out of range.");
        }

        // Calculate the DateTime
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddDays(julianDay - 2440588) // Subtract the Julian day of the Unix epoch
            .AddTicks(nanosecondsWithinDay / 100); // Convert nanoseconds to ticks

        return dateTime;
    }

    // stack overflow version
    public DateTime DateFromInt96Timestamp(BigInteger int96Timestamp)
    {
        BigInteger julianCalendarDays = int96Timestamp >> (8 * 8);
        BigInteger time = int96Timestamp & BigInteger.Parse("FFFFFFFFFFFFFFFF", NumberStyles.HexNumber);
        BigInteger microseconds = time / 1_000;
        long linuxEpoch = 2_440_588;

        return new DateTime(1970, 1, 1)
            .AddDays((long)julianCalendarDays - linuxEpoch)
            .AddMilliseconds((double)microseconds / 1_000);
    }

    public static DateTime DateFromInt96Timestamp2(BigInteger int96Timestamp)
    {
        BigInteger julianCalendarDays = int96Timestamp >> (8 * 8);
        BigInteger time = int96Timestamp & BigInteger.Parse("FFFFFFFFFFFFFFFF", NumberStyles.HexNumber);
        BigInteger microseconds = time / 1_000;

        long linuxEpoch = 2_440_588;
        return new DateTime(1970, 1, 1)
            .AddDays((long)(julianCalendarDays - linuxEpoch))
            .AddMilliseconds((long)(microseconds / 1_000)); // Cast to long before adding
    }
}
