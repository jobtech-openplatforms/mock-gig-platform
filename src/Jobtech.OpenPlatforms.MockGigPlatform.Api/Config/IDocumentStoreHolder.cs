using Raven.Client.Documents;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Config
{
    public interface IDocumentStoreHolder
    {
        IDocumentStore Store { get;  }
    }
}
