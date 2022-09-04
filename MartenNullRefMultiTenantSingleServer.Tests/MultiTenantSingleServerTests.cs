using System.Threading.Tasks;
using Marten;
using Marten.Exceptions;
using Npgsql;
using Weasel.Core;

namespace MartenNullRefMultiTenantSingleServer;

public class UnitTest1
{
  [Fact]
  public async Task ShouldThrowTenantExceptionOnMissingTenantForSession()
  {
    var connectionString = new NpgsqlConnectionStringBuilder()
    {
      Pooling = false,
      Port = 5435,
      Host = "localhost",
      CommandTimeout = 20,
      Database = "postgres",
      Password = "123456",
      Username = "postgres"
    }.ToString();
    var store = DocumentStore.For(options =>
    {
      options.MultiTenantedWithSingleServer(connectionString);
      options.AutoCreateSchemaObjects = AutoCreate.All;
    });

    await Assert.ThrowsAsync<DefaultTenantUsageDisabledException>(async () =>
      {
        await using var lightweightSession = store.LightweightSession();
      })
      ;
  }
}
