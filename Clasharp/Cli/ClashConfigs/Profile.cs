﻿using YamlDotNet.Serialization;

namespace Clasharp.Cli.ClashConfigs;

public class Profile
{
    [YamlMember(Alias = "store-selected")]
    public bool StoreSelected { get; set; }

    [YamlMember(Alias = "store-fake-ip")]
    public bool StoreFakeIP { get; set; }
}