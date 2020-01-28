using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Config
{
    public class RavenDatabaseCheckService : BackgroundService
    {
        private readonly IDocumentStore store;

        public RavenDatabaseCheckService(IDocumentStoreHolder documentStoreHolder)
        {
            this.store = documentStoreHolder.Store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await EnsureDatabaseExistsAsync(store, "gigdata", true);
            await EnsureDatabaseExistsAsync(store, "mockgigplatform", true);
        }

        public async Task EnsureDatabaseExistsAsync(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database = database ?? store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                await store.Maintenance.ForDatabase(database).SendAsync(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                if (createDatabaseIfNotExists == false)
                    throw;

                try
                {
                    await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(database)));
                }
                catch (ConcurrencyException)
                {
                }
            }
        }
    }
}
