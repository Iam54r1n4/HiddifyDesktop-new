﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClashGui.Clash.Models.Rules;
using ClashGui.Cli;
using ClashGui.Services.Base;

namespace ClashGui.Services;

public class ProxyRuleService : BaseListService<RuleInfo>, IProxyRuleService
{
    public ProxyRuleService(IClashCli clashCli) : base(clashCli)
    {
    }

    protected override async Task<List<RuleInfo>> GetObj()
    {
        return (await GlobalConfigs.ClashControllerApi.GetRules())?.Rules ?? new List<RuleInfo>();
    }
}