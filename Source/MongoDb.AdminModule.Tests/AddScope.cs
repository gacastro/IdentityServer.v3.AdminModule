﻿using System.Linq;
using System.Management.Automation;
using IdentityServer.Core.MongoDb;
using Thinktecture.IdentityServer.Core.Models;
using Thinktecture.IdentityServer.Core.Services;
using Xunit;

namespace MongoDb.AdminModule.Tests
{
    public class AddScope : IClassFixture<PowershellAdminModuleFixture>
    {
        private readonly PowerShell _ps;
        private readonly IScopeStore _scopeStore;

        [Fact]
        public void VerifyAdd()
        {
            
            Assert.Empty(_scopeStore.GetScopesAsync(false).Result);
            _ps.Invoke();

            var result = _scopeStore.GetScopesAsync(false).Result.ToArray();
            Assert.Equal(1, result.Length);
            var scope = result.Single();
            Assert.Equal("unit_test_scope", scope.Name);
            Assert.Equal("displayName", scope.DisplayName);
            Assert.Equal("claim description", scope.Description);
            Assert.Equal("customRuleName", scope.ClaimsRule);
            Assert.True(scope.Emphasize);
            Assert.True(scope.Enabled);
            Assert.True(scope.IncludeAllClaimsForUser);
            Assert.True(scope.Required);
            Assert.True(scope.ShowInDiscoveryDocument);
            Assert.Equal(ScopeType.Identity, scope.Type);
            Assert.Equal(2, scope.Claims.Count());
            var first = scope.Claims.OrderBy(x => x.Name).First();
            Assert.Equal("unit_test_claim1", first.Name);
            Assert.Equal("Sample description for unit test", first.Description);
            Assert.True(first.AlwaysIncludeInIdToken);

            var second = scope.Claims.OrderBy(x => x.Name).Skip(1).First();
            Assert.Equal("unit_test_claim2", second.Name);
            Assert.Equal("Sample description", second.Description);
            Assert.False(second.AlwaysIncludeInIdToken);

        }

        public AddScope(PowershellAdminModuleFixture data)
        {
            _ps = data.PowerShell;
            var script = data.LoadScript(this);
            var database = data.Database;
            _ps.AddScript(script).AddParameter("Database", database);
            var adminService = data.Factory.Resolve<IAdminService>();
            adminService.CreateDatabase();
            _scopeStore = data.Factory.Resolve<IScopeStore>();
        }
    }
}