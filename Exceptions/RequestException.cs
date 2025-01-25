using System;

namespace desktop.Exceptions;

public class RequestException(string message): Exception(message)
{
}