﻿using System;
using System.Collections.Generic;

namespace Flora;

public partial class UserAccount
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string FullName { get; set; }

    public string Role { get; set; }
}