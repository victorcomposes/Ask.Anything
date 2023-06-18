﻿using System.ComponentModel;

namespace Ask.Anything.Blazor.Server.Models;

public enum Sender
{
    [Description("")]
    USER = 1,
    [Description("")]
    AI = 2,
    [Description("")]
    SYSTEM = 3,
}