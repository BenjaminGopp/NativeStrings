﻿using System;
using System.Runtime.ExceptionServices;

namespace NativeStrings
{
  public class NativeString
  {
    internal NativeString(IntPtr stringPtr, int length)
    {
      StringPtr = stringPtr;
      Length = length;
    }

    private IntPtr StringPtr { get; }
    public int Length { get; }


    public static implicit operator ReadOnlySpan<char>(NativeString nativeString)
    {
      unsafe
      {
        return new ReadOnlySpan<char>(nativeString.StringPtr.ToPointer(), nativeString.Length);
      }
    }


    public override string ToString()
    {
      ReadOnlySpan<char> s = this;
      return s.ToString();
    }
  }
}
