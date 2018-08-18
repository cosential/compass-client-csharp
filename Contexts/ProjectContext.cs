using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;

namespace Cosential.Integrations.Compass.Client.Contexts
{
    public class ProjectContext : ICompassContext<Project>
    {
        private readonly CompassClient _client;
        public ProjectContext(CompassClient client)
        {
            _client = client;
        }

        #region CRUD FOR /projects

        public IList<Project> Create(IEnumerable<Project> project)
        {
            var request = _client.NewRequest("projects", Method.POST);
            request.AddBody(project);

            var results = _client.Execute<List<Project>>(request);
            return results.Data;
        }

        public async Task<IList<Project>> CreateAsync(IEnumerable<Project> project, CancellationToken cancel)
        {
            var request = _client.NewRequest("projects", Method.POST);
            request.AddBody(project);

            var results = await _client.ExecuteAsync<List<Project>>(request, cancel);
            return results.Data;
        }

        public Project Create(Project project)
        {
            return Create(new[] { project }).FirstOrDefault();
        }

        public async Task<Project> CreateAsync(Project project, CancellationToken cancel)
        {
            var result = await CreateAsync(new[] { project }, cancel);
            return result.FirstOrDefault();
        }

        public Project Get(int projectId)
        {
            var request = _client.NewRequest("projects/{id}");
            request.AddUrlSegment("id", projectId.ToString());

            var results = _client.Execute<Project>(request);
            return results.Data;
        }

        public async Task<Project> GetAsync(int projectId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("projects/{id}");
            request.AddUrlSegment("id", projectId.ToString());

            var results = await _client.ExecuteAsync<Project>(request, cancelToken);
            return results.Data;
        }

        public IList<Project> List(int from, int take)
        {
            var request = _client.NewRequest("projects");
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("take", take.ToString());

            var results = _client.Execute<List<Project>>(request);
            return results.Data;
        }

        public IList<ChangeEvent> GetChanges(byte[] version = null, bool includeDeleted = false)
        {
            var task = GetChangesAsync(version, includeDeleted, CancellationToken.None);
            task.RunSynchronously();
            return task.Result;
        }

        public async Task<List<ChangeEvent>> GetChangesAsync(byte[] version, bool includeDeleted, CancellationToken cancel)
        {
            var request = _client.NewRequest("projects/changes");
            if (version != null) request.AddQueryParameter("version", Convert.ToBase64String(version));
            if (includeDeleted) request.AddQueryParameter("includeDeleted", true.ToString());
            var results = await _client.ExecuteAsync<List<ChangeEvent>>(request, cancel);
            return results.Data;
        }

        public Project Update(Project project)
        {
            var request = _client.NewRequest("projects/{id}", Method.PUT);
            request.AddUrlSegment("id", project.ProjectId.ToString());
            request.AddBody(project);

            var results = _client.Execute<Project>(request);
            return results.Data;
        }

        public async Task<Project> UpdateAsync(Project project, CancellationToken cancel)
        {
            var request = _client.NewRequest("projects/{id}", Method.PUT);
            request.AddUrlSegment("id", project.ProjectId.ToString());
            request.AddBody(project);

            var results = await _client.ExecuteAsync<Project>(request, cancel);
            return results.Data;
        }

        public void Delete(int projectId)
        {
            var request = _client.NewRequest("projects/{id}", Method.DELETE);
            request.AddUrlSegment("id", projectId.ToString());

            var results = _client.Execute<Project>(request);
        }

        public async Task DeleteAsync(int projectId, CancellationToken cancelToken)
        {
            var request = _client.NewRequest("projects/{id}", Method.DELETE);
            request.AddUrlSegment("id", projectId.ToString());

            await _client.ExecuteAsync(request, cancelToken);
        }

        #endregion

        #region PROJECT IMAGES
        public IEnumerable<ProjectImageMetadata> GetProjectImageData(Project project)
        {
            var request = _client.NewRequest("projects/{id}/images");
            request.AddUrlSegment("id", project.ProjectId.ToString());
            var result = _client.Execute<List<ProjectImageMetadata>>(request);
            return result.Data;
        }

        public bool HasImage(Project project)
        {
            return GetProjectImageData(project).Any();
        }

        public bool UploadImage(Project project, string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl) || HasImage(project)) return false;

            var request = _client.NewRequest("/images/projects/{id}", Method.POST);
            request.AddUrlSegment("id", project.ProjectId.ToString());
            request.AddQueryParameter("defaultImage", "true");
            request.AddQueryParameter("url", photoUrl);
            request.AddHeader("Content-Type", "application/json");

            var result = _client.Execute(request);
            return result.ResponseStatus == ResponseStatus.Completed;
        }

        #endregion

        public async Task<TM> GetMetadataAync<TM>(MetadataScope scope, int entityId, CancellationToken cancellationToken)
        {
            var request = _client.NewRequest("projects/{id}/metadata/{scope}");
            request.AddUrlSegment("id", entityId.ToString());
            request.AddUrlSegment("scope", scope.ToString());

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken);
            return result.Data;
        }

        public async Task<TM> PutMetadataAsync<TM>(MetadataScope scope, int entityId, TM data, CancellationToken cancellationToken)
        {
            var request = _client.NewRequest("projects/{id}/metadata/{scope}", Method.PUT);
            request.AddUrlSegment("id", entityId.ToString());
            request.AddUrlSegment("scope", scope.ToString());
            request.AddBody(data);

            var result = await _client.ExecuteAsync<TM>(request, cancellationToken);
            return result.Data;
        }

        #region SEARCH ENDPOINT

        public List<Project> Search(string textToSearch, int from = 0, int take = 50)
        {
            var request = _client.NewRequest("projects/search");
            request.AddQueryParameter("q", textToSearch);
            request.AddQueryParameter("from", from.ToString());
            request.AddQueryParameter("take", take.ToString());

            var results = _client.Execute<List<Project>>(request);
            return results.Data;
        }

        #endregion

        #region CONVENIENCE FUNCTIONS

        public Project GetByProjectNumber(string projectNumber)
        {
            var items = Search($"ProjectNumber.raw:{projectNumber}");
            return items.FirstOrDefault(p => p.ProjectNumber == projectNumber);
        }

        public UpsertResult<Project> Upsert(Project project)
        {
            return new UpsertResult<Project>
            {
                Action = (project.ProjectId.HasValue && project.ProjectId.Value > 0) ? UpsertAction.Updated : UpsertAction.Created,
                Data = (project.ProjectId.HasValue && project.ProjectId.Value > 0) ? Update(project) : Create(project)
            };
        }

        public async Task<UpsertResult<Project>> UpsertAsync(Project project, CancellationToken cancel)
        {
            var result = new UpsertResult<Project>();

            if (project.ProjectId.HasValue && project.ProjectId.Value > 0)
            {
                result.Action = UpsertAction.Updated;
                result.Data = await UpdateAsync(project, cancel);
            }
            else
            {
                result.Action = UpsertAction.Created;
                result.Data = await CreateAsync(project, cancel);
            }

            return result;
        }

        public UpsertResult<Project> UpsertByProjectNumber(Project project)
        {
            var found = GetByProjectNumber(project.ProjectNumber);
            if (found != null) project.ProjectId = found.ProjectId;

            return Upsert(project);
        }

        #endregion

        #region ASSOCIATE SUB-OBJECTS
        public List<Office> AddOfficeToProject(int projectId, string officeName)
        {
            //Data to post
            var data = new List<Office>();

            //Look up office
            var findOfficeRequest = _client.NewRequest($"firmorgs/offices");
            findOfficeRequest.AddQueryParameter("q", $"OfficeName.raw:\"{officeName}\"");
            var findOfficeResults = _client.Execute<List<Office>>(findOfficeRequest);

            if (findOfficeResults.Data.Any())
            {
                data.Add(findOfficeResults.Data.First());
            }
            else
            {
                //Add new office
                var addOfficeRequest = _client.NewRequest($"firmorgs/offices", Method.POST);
                addOfficeRequest.AddBody(new List<Office> { new Office { OfficeName = officeName } });
                var addOfficeResponse = _client.Execute<List<Office>>(addOfficeRequest);
                if (addOfficeResponse.Data.Any()) data.Add(addOfficeResponse.Data.First());
                else throw new Exception($"Could not find or create an office named {officeName} in Cosential");
            }

            //Associate the office to the project
            var request = _client.NewRequest($"projects/{projectId}/offices", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<Office>>(request);
            return results.Data;
        }

        #endregion
    }
}