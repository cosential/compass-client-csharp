using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosential.Integrations.Compass.Client.Models;
using RestSharp;

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

        #region Office
        public IList<Office> AddOfficeToProject(int projectId, string officeName)
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

        #endregion Office

        #region Construction Cost
        ///<summary>Returns construction costs for all the projects.</summary>
        public IList<ProjectConstructionCost> GetConstructionCosts()
        {
            var request = _client.NewRequest("/projects/ConstructionCosts");
            var results = _client.Execute<List<ProjectConstructionCost>>(request);

            return results.Data;
        }

        ///<summary>Returns construction cost for a specific project.</summary>
        public IList<ProjectConstructionCost> GetConstructionCost(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/constructioncosts");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectConstructionCost>>(request);
            return results.Data;
        }

        ///<summary>
        ///Associates new or updates construction costs for a project. 
        ///**Note for Updates: All the values for 'projectConstructionCost' must be set. You may get hold of the values by calling the GetConstructionCost(projectId).
        ///</summary>
        public ProjectConstructionCost AddConstructioCostToProject(int projectId, ProjectConstructionCost projectConstructionCost)
        {
            var request = _client.NewRequest("projects/{projectId}/constructioncosts", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(projectConstructionCost);

            var results = _client.Execute<ProjectConstructionCost>(request);
            return results.Data;
        }

        ///<summary>Deletes construction costs for a project.</summary>
        public void DeleteConstructionCost(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/constructioncosts", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(new ProjectConstructionCost { });
            _client.Execute(request);
        }

        #endregion Construction Cost

        #region Primary Category

        ///<summary>Returns all the values from the primary categories valuelist.</summary>
        public IList<PrimaryCategory> GetPrimaryCategories()
        {
            var request = _client.NewRequest("/projects/primarycategories");
            var results = _client.Execute<List<PrimaryCategory>>(request);

            return results.Data;
        }

        ///<summary>Returns primary category for a specific project.</summary>
        public IList<PrimaryCategory> GetPrimaryCategories(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/primarycategories");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<PrimaryCategory>>(request);
            return results.Data;
        }

        ///<summary>Returns specific primary category for a project.</summary>
        public PrimaryCategory GetPrimaryCategory(int projectId, int primaryCategoryId)
        {
            var request = _client.NewRequest("projects/{projectId}/primarycategories/{primaryCategoryId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("primaryCategoryId", primaryCategoryId.ToString());

            var results = _client.Execute<PrimaryCategory>(request);
            return results.Data;
        }

        ///<summary>Associates new or existing primary category for a project.</summary>
        public IList<PrimaryCategory> AddPrimaryCategoryToProject(int projectId, string categoryName)
        {
            //Data to post
            var data = new List<PrimaryCategory>();

            //Look up primary category
            var findCategoryRequest = _client.NewRequest("/projects/primarycategories");
            findCategoryRequest.AddQueryParameter("q", $"CategoryName.raw:\"{categoryName}\"");
            var findCategoryResults = _client.Execute<List<PrimaryCategory>>(findCategoryRequest);

            if (findCategoryResults.Data.Any())
            {
                data.Add(findCategoryResults.Data.First());
            }
            else
            {
                //Add new primary category
                var addCategoryRequest = _client.NewRequest("/projects/primarycategories", Method.POST);
                addCategoryRequest.AddBody(new List<PrimaryCategory> { new PrimaryCategory { CategoryName = categoryName } });
                var addCategoryResponse = _client.Execute<List<PrimaryCategory>>(addCategoryRequest);
                if (addCategoryResponse.Data.Any()) data.Add(addCategoryResponse.Data.First());
                else throw new Exception($"Could not find or create a primary category named {categoryName} in Cosential");
            }

            //Associate the primary category to the project
            var request = _client.NewRequest($"projects/{projectId}/primarycategories", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<PrimaryCategory>>(request);
            return results.Data;
        }

        ///<summary>Deletes primary category for a project.</summary>
        public void DeletePrimaryCategory(int projectId, string primaryCategoryId)
        {
            var request = _client.NewRequest("projects/{projectId}/primarycategories/{primaryCategoryId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("primaryCategoryId", primaryCategoryId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the primary categories for a project.</summary>
        public void DeletePrimaryCategories(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/primarycategories", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Primary Category

        #region Secondary Category

        ///<summary>Returns all the values from the secondary categories valuelist.</summary>
        public IList<SecondaryCategory> GetSecondaryCategories()
        {
            var request = _client.NewRequest("/projects/secondarycategories");
            var results = _client.Execute<List<SecondaryCategory>>(request);

            return results.Data;
        }

        ///<summary>Returns secondary category for a specific project.</summary>
        public IList<SecondaryCategory> GetSecondaryCategories(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/secondarycategories");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<SecondaryCategory>>(request);
            return results.Data;
        }

        ///<summary>Returns specific secondary category for a project.</summary>
        public SecondaryCategory GetSecondaryCategory(int projectId, int secondaryCategoryId)
        {
            var request = _client.NewRequest("projects/{projectId}/secondarycategories/{secondaryCategoryId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("secondaryCategoryId", secondaryCategoryId.ToString());

            var results = _client.Execute<SecondaryCategory>(request);
            return results.Data;
        }

        ///<summary>Associates new or existing secondary category for a project.</summary>
        public IList<SecondaryCategory> AddSecondaryCategoryToProject(int projectId, string categoryName)
        {
            //Data to post
            var data = new List<SecondaryCategory>();

            //Look up secondary category
            var findCategoryRequest = _client.NewRequest("/projects/secondarycategories");
            findCategoryRequest.AddQueryParameter("q", $"SecondaryCategoryName.raw:\"{categoryName}\"");
            var findCategoryResults = _client.Execute<List<SecondaryCategory>>(findCategoryRequest);

            if (findCategoryResults.Data.Any())
            {
                data.Add(findCategoryResults.Data.First());
            }
            else
            {
                //Add new secondary category
                var addCategoryRequest = _client.NewRequest("/projects/secondarycategories", Method.POST);
                addCategoryRequest.AddBody(new List<SecondaryCategory> { new SecondaryCategory { SecondaryCategoryName = categoryName } });
                var addCategoryResponse = _client.Execute<List<SecondaryCategory>>(addCategoryRequest);
                if (addCategoryResponse.Data.Any()) data.Add(addCategoryResponse.Data.First());
                else throw new Exception($"Could not find or create a secondary category named {categoryName} in Cosential");
            }

            //Associate the secondary category to the project
            var request = _client.NewRequest($"projects/{projectId}/secondarycategories", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<SecondaryCategory>>(request);
            return results.Data;
        }

        ///<summary>Deletes secondary category for a project.</summary>
        public void DeleteSecondaryCategory(int projectId, string secondaryCategoryId)
        {
            var request = _client.NewRequest("projects/{projectId}/secondarycategories/{secondaryCategoryId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("secondaryCategoryId", secondaryCategoryId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the secondary categories for a project.</summary>
        public void DeleteSecondaryCategories(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/secondarycategories", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Secondary Category

        #region Service Type

        ///<summary>Returns all the values from the project service types valuelist.</summary>
        public IList<ProjectServiceType> GetServiceTypes()
        {
            var request = _client.NewRequest("/projects/servicetypes");
            var results = _client.Execute<List<ProjectServiceType>>(request);

            return results.Data;
        }

        ///<summary>Returns service type for a specific project.</summary>
        public IList<ProjectServiceType> GetServiceTypes(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/servicetypes");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectServiceType>>(request);
            return results.Data;
        }

        ///<summary>Returns specific service type for a project.</summary>
        public ProjectServiceType GetServiceType(int projectId, int serviceTypeId)
        {
            var request = _client.NewRequest("projects/{projectId}/servicetypes/{serviceTypeId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("serviceTypeId", serviceTypeId.ToString());

            var results = _client.Execute<ProjectServiceType>(request);
            return results.Data;
        }

        ///<summary>Associates new or existing service type for a project.</summary>
        public IList<ProjectServiceType> AddServiceTypeToProject(int projectId, string serviceTypeName)
        {
            //Data to post
            var data = new List<ProjectServiceType>();

            //Look up service type
            var findRequest = _client.NewRequest("/projects/servicetypes");
            findRequest.AddQueryParameter("q", $"ServiceTypeName.raw:\"{serviceTypeName}\"");
            var findResults = _client.Execute<List<ProjectServiceType>>(findRequest);

            if (findResults.Data.Any())
            {
                data.Add(findResults.Data.First());
            }
            else
            {
                //Add new service type
                var addRequest = _client.NewRequest("/projects/servicetypes", Method.POST);
                addRequest.AddBody(new List<ProjectServiceType> { new ProjectServiceType { ServiceTypeName = serviceTypeName } });
                var addResponse = _client.Execute<List<ProjectServiceType>>(addRequest);
                if (addResponse.Data.Any()) data.Add(addResponse.Data.First());
                else throw new Exception($"Could not find or create a service type named {serviceTypeName} in Cosential");
            }

            //Associate the service type to the project
            var request = _client.NewRequest($"projects/{projectId}/servicetypes", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<ProjectServiceType>>(request);
            return results.Data;
        }

        ///<summary>Deletes service type for a project.</summary>
        public void DeleteServiceType(int projectId, string serviceTypeId)
        {
            var request = _client.NewRequest("projects/{projectId}/servicetypes/{serviceTypeId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("serviceTypeId", serviceTypeId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the service types for a project.</summary>
        public void DeleteServiceTypes(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/servicetypes", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Service Type

        #region Contract Type

        ///<summary>Returns all the values from the project contract valuelist.</summary>
        public IList<ContractType> GetContractTypes()
        {
            var request = _client.NewRequest("/projects/contracttypes");
            var results = _client.Execute<List<ContractType>>(request);

            return results.Data;
        }

        ///<summary>Returns contract type for a specific project.</summary>
        public IList<ContractType> GetContractTypes(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/contracttypes");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ContractType>>(request);
            return results.Data;
        }

        ///<summary>Returns specific contract type for a project.</summary>
        public ContractType GetContractType(int projectId, int contractTypeId)
        {
            var request = _client.NewRequest("projects/{projectId}/contracttypes/{contractTypeId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("contractTypeId", contractTypeId.ToString());

            var results = _client.Execute<ContractType>(request);
            return results.Data;
        }

        ///<summary>Associates new or existing contract type for a project.</summary>
        public IList<ContractType> AddContractTypeToProject(int projectId, string contractTypeName)
        {
            //Data to post
            var data = new List<ContractType>();

            //Look up serice type
            var findRequest = _client.NewRequest("/projects/contracttypes");
            findRequest.AddQueryParameter("q", $"Name.raw:\"{contractTypeName}\"");
            var findResults = _client.Execute<List<ContractType>>(findRequest);

            if (findResults.Data.Any())
            {
                data.Add(findResults.Data.First());
            }
            else
            {
                //Add new contract type
                var addRequest = _client.NewRequest("/projects/contracttypes", Method.POST);
                addRequest.AddBody(new List<ContractType> { new ContractType { Name = contractTypeName } });
                var addResponse = _client.Execute<List<ContractType>>(addRequest);
                if (addResponse.Data.Any()) data.Add(addResponse.Data.First());
                else throw new Exception($"Could not find or create a contract type named {contractTypeName} in Cosential");
            }

            //Associate the contract type to the project
            var request = _client.NewRequest($"projects/{projectId}/contracttypes", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<ContractType>>(request);
            return results.Data;
        }

        ///<summary>Deletes contract type for a project.</summary>
        public void DeleteContractType(int projectId, string contractTypeId)
        {
            var request = _client.NewRequest("projects/{projectId}/contracttypes/{contractTypeId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("contractTypeId", contractTypeId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the contract types for a project.</summary>
        public void DeleteContractTypes(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/contracttypes", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Contract Type

        #region Call Log

        ///<summary>Returns call logs.</summary>
        public IList<CallLog> GetCallLogs()
        {
            var request = _client.NewRequest("/calllogs");
            var results = _client.Execute<List<CallLog>>(request);

            return results.Data;
        }

        ///<summary>Creates new call logs.</summary>
        public IList<CallLog> AddCallLog(List<CallLog> callLogs)
        {
            var request = _client.NewRequest("calllogs", Method.POST);
            request.AddBody(callLogs);

            var results = _client.Execute<List<CallLog>>(request);
            return results.Data;
        }

        ///<summary>Deletes a call log.</summary>
        public void DeleteCallLog(string callLogId)
        {
            var request = _client.NewRequest("calllogs/{callLogId}", Method.DELETE);
            request.AddUrlSegment("callLogId", callLogId.ToString());
            _client.Execute(request);
        }

        #endregion Call Log

        #region Owner Client

        ///<summary>Returns owner client for a specific project.</summary>
        public IList<ProjectOwnerClient> GetOwnerClients(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclient");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectOwnerClient>>(request);
            return results.Data;
        }

        ///<summary>Returns specific owner client for a project.</summary>
        public ProjectOwnerClient GetOwnerClient(int projectId, int ownerClientId)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclient/{ownerClientId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("ownerClientId", ownerClientId.ToString());

            var results = _client.Execute<ProjectOwnerClient>(request);
            return results.Data;
        }

        ///<summary>Associates new owner clients (company) for a project.</summary>
        public IList<ProjectOwnerClient> AddOwnerClientsToProject(int projectId, List<ProjectOwnerClient> ownerClients)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclient", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(ownerClients);

            var results = _client.Execute<List<ProjectOwnerClient>>(request);
            return results.Data;
        }

        ///<summary>Deletes owner client (company) for a project.</summary>
        public void DeleteOwnerClient(int projectId, string ownerClientId)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclient/{ownerClientId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("ownerClientId", ownerClientId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the owner clients (company) for a project.</summary>
        public void DeleteOwnerClients(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclient", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Owner Client

        #region Owner Client Contact

        ///<summary>Returns owner client contacts for a specific project.</summary>
        public IList<ProjectOwnerClientContact> GetOwnerClientContacts(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclientcontacts");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectOwnerClientContact>>(request);
            return results.Data;
        }

        ///<summary>Returns specific owner client contact for a project.</summary>
        public ProjectOwnerClientContact GetOwnerClientContact(int projectId, int ownerClientContactId)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclientcontacts/{ownerClientContactId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("ownerClientContactId", ownerClientContactId.ToString());

            var results = _client.Execute<ProjectOwnerClientContact>(request);
            return results.Data;
        }

        ///<summary>Associates new owner client contacts for a project.</summary>
        public IList<ProjectOwnerClientContact> AddOwnerClientContactsToProject(int projectId, List<ProjectOwnerClientContact> ownerClientContacts)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclientcontacts", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(ownerClientContacts);

            var results = _client.Execute<List<ProjectOwnerClientContact>>(request);
            return results.Data;
        }

        ///<summary>Deletes owner client contact for a project.</summary>
        public void DeleteOwnerClientContact(int projectId, string ownerClientContactId)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclientcontacts/{ownerClientContactId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("ownerClientContactId", ownerClientContactId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the owner client contacts for a project.</summary>
        public void DeleteOwnerClientContacts(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/ownerclientcontacts", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Owner Client Contact

        #region Description

        ///<summary>Returns descriptions for a specific project.</summary>
        public IList<ProjectDescription> GetDescriptions(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/descriptions");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectDescription>>(request);
            return results.Data;
        }

        ///<summary>Returns specific description for a project.</summary>
        public ProjectDescription GetDescription(int projectId, int descriptionId)
        {
            var request = _client.NewRequest("projects/{projectId}/descriptions/{descriptionId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("descriptionId", descriptionId.ToString());

            var results = _client.Execute<ProjectDescription>(request);
            return results.Data;
        }

        ///<summary>Associates new descriptions for a project.</summary>
        public IList<ProjectDescription> AddDescriptionsToProject(int projectId, List<ProjectDescription> descriptions)
        {
            var request = _client.NewRequest("projects/{projectId}/descriptions", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(descriptions);

            var results = _client.Execute<List<ProjectDescription>>(request);
            return results.Data;
        }

        ///<summary>Deletes description for a project.</summary>
        public void DeleteDescription(int projectId, string descriptionId)
        {
            var request = _client.NewRequest("projects/{projectId}/descriptions/{descriptionId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("descriptionId", descriptionId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the descriptions for a project.</summary>
        public void DeleteDescriptions(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/descriptions", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Description

        #region Role

        ///<summary>Returns all the values from the project roles valuelist.</summary>
        public IList<Role> GetRoles()
        {
            var request = _client.NewRequest("/projects/role");
            var results = _client.Execute<List<Role>>(request);

            return results.Data;
        }

        ///<summary>Returns project role for a specific project.</summary>
        public Role GetRole(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/role");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<Role>(request);
            return results.Data;
        }

        ///<summary>Associates new or existing role for a project.</summary>
        public IList<Role> AddRoleToProject(int projectId, string roleName)
        {
            //Data to post
            var data = new List<Role>();

            //Look up role
            var findRequest = _client.NewRequest("/projects/role");
            findRequest.AddQueryParameter("q", $"RoleName.raw:\"{roleName}\"");
            var findResults = _client.Execute<List<Role>>(findRequest);

            if (findResults.Data.Any())
            {
                data.Add(findResults.Data.First());
            }
            else
            {
                //Add new role
                var addRequest = _client.NewRequest("/projects/role", Method.POST);
                addRequest.AddBody(new List<Role> { new Role { RoleName = roleName } });
                var addResponse = _client.Execute<List<Role>>(addRequest);
                if (addResponse.Data.Any()) data.Add(addResponse.Data.First());
                else throw new Exception($"Could not find or create a contract type named {roleName} in Cosential");
            }

            //Associate the role to the project
            var request = _client.NewRequest($"projects/{projectId}/role", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<Role>>(request);
            return results.Data;
        }

        ///<summary>Deletes role for a project.</summary>
        public void DeleteRole(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/role", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Role

        #region Change Order        

        ///<summary>Returns change orders for a specific project.</summary>
        public IList<ProjectChangeOrder> GetChangeOrders(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/changeorders");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectChangeOrder>>(request);
            return results.Data;
        }

        ///<summary>Returns specific change order for a project.</summary>
        public ProjectChangeOrder GetChangeOrder(int projectId, int changeOrderId)
        {
            var request = _client.NewRequest("projects/{projectId}/changeorders/{changeOrderId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("changeOrderId", changeOrderId.ToString());

            var results = _client.Execute<ProjectChangeOrder>(request);
            return results.Data;
        }

        ///<summary>Associates new change orders for a project.</summary>
        public IList<ProjectChangeOrder> AddChangeOrdersToProject(int projectId, List<ProjectChangeOrder> changeOrders)
        {
            var request = _client.NewRequest("projects/{projectId}/changeorders", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(changeOrders);

            var results = _client.Execute<List<ProjectChangeOrder>>(request);
            return results.Data;
        }

        ///<summary>Deletes change order for a project.</summary>
        public void DeleteChangeOrder(int projectId, string changeOrderId)
        {
            var request = _client.NewRequest("projects/{projectId}/changeorders/{changeOrderId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("changeOrderId", changeOrderId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the change orders for a project.</summary>
        public void DeleteChangeOrders(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/changeorders", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Change Order

        #region Invoice

        ///<summary>Returns invoices for a specific project.</summary>
        public IList<ProjectInvoice> GetInvoices(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/invoices");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectInvoice>>(request);
            return results.Data;
        }

        ///<summary>Returns specific invoice for a project.</summary>
        public ProjectInvoice GetInvoice(int projectId, int invoiceId)
        {
            var request = _client.NewRequest("projects/{projectId}/invoices/{invoiceId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("invoiceId", invoiceId.ToString());

            var results = _client.Execute<ProjectInvoice>(request);
            return results.Data;
        }

        ///<summary>Associates new invoices for a project.</summary>
        public IList<ProjectInvoice> AddInvoicesToProject(int projectId, List<ProjectInvoice> invoices)
        {
            var request = _client.NewRequest("projects/{projectId}/invoices", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(invoices);

            var results = _client.Execute<List<ProjectInvoice>>(request);
            return results.Data;
        }

        ///<summary>Deletes invoice for a project.</summary>
        public void DeleteInvoice(int projectId, string invoiceId)
        {
            var request = _client.NewRequest("projects/{projectId}/invoices/{invoiceId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("invoiceId", invoiceId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the invoices for a project.</summary>
        public void DeleteInvoices(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/invoices", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Invoice

        #region Opportunity

        ///<summary>Returns opportunity for a specific project.</summary>
        public Opportunity GetOpportunity(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/opportunity");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<Opportunity>(request);
            return results.Data;
        }

        ///<summary>Associates new opportunity for a project.</summary>
        public Opportunity AddOpportunityToProject(int projectId, Opportunity opportunity)
        {
            var request = _client.NewRequest("projects/{projectId}/opportunity", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(opportunity);

            var results = _client.Execute<Opportunity>(request);
            return results.Data;
        }

        ///<summary>Deletes opportunity for a project.</summary>
        public void DeleteOpportunity(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/opportunity", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Opportunity

        #region Staff Team        

        ///<summary>Returns staff team for a specific project.</summary>
        public IList<ProjectStaffTeam> GetStaffTeam(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/staffteam");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectStaffTeam>>(request);
            return results.Data;
        }

        ///<summary>Returns specific staff team member for a project.</summary>
        public ProjectStaffTeam GetStaffTeam(int projectId, int staffTeamId)
        {
            var request = _client.NewRequest("projects/{projectId}/staffteam/{staffTeamId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("staffTeamId", staffTeamId.ToString());

            var results = _client.Execute<ProjectStaffTeam>(request);
            return results.Data;
        }

        ///<summary>Associates new staff team member for a project.</summary>
        public IList<ProjectStaffTeam> AddStaffTeamToProject(int projectId, List<ProjectStaffTeam> staffTeamMembers)
        {
            var request = _client.NewRequest("projects/{projectId}/staffteam", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(staffTeamMembers);

            var results = _client.Execute<List<ProjectStaffTeam>>(request);
            return results.Data;
        }

        ///<summary>Deletes staff team member for a project.</summary>
        public void DeleteStaffTeam(int projectId, string staffTeamId)
        {
            var request = _client.NewRequest("projects/{projectId}/staffteam/{staffTeamId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("staffTeamId", staffTeamId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the staff team members for a project.</summary>
        public void DeleteStaffTeam(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/staffteam", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Staff Team

        #region Project Financial Status

        ///<summary>Returns all the values from the project financial status valuelist.</summary>
        public IList<ProjectFinancialStatus> GetFinancialStatus()
        {
            var request = _client.NewRequest("/projects/projectfinancialstatus");
            var results = _client.Execute<List<ProjectFinancialStatus>>(request);

            return results.Data;
        }

        ///<summary>Returns financial status for a specific project.</summary>
        public ProjectFinancialStatus GetFinancialStatus(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/projectfinancialstatus");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<ProjectFinancialStatus>(request);
            return results.Data;
        }

        ///<summary>Associates new or existing financial status for a project.</summary>
        public IList<ProjectFinancialStatus> AddFinancialStatusToProject(int projectId, string statusName)
        {
            //Data to post
            var data = new List<ProjectFinancialStatus>();

            //Look up financial status
            var findRequest = _client.NewRequest("/projects/projectfinancialstatus");
            findRequest.AddQueryParameter("q", $"Name.raw:\"{statusName}\"");
            var findResults = _client.Execute<List<ProjectFinancialStatus>>(findRequest);

            if (findResults.Data.Any())
            {
                data.Add(findResults.Data.First());
            }
            else
            {
                //Add new financial status
                var addRequest = _client.NewRequest("/projects/projectfinancialstatus", Method.POST);
                addRequest.AddBody(new List<ProjectFinancialStatus> { new ProjectFinancialStatus { Name = statusName } });
                var addResponse = _client.Execute<List<ProjectFinancialStatus>>(addRequest);
                if (addResponse.Data.Any()) data.Add(addResponse.Data.First());
                else throw new Exception($"Could not find or create a contract type named {statusName} in Cosential");
            }

            //Associate the financial status to the project
            var request = _client.NewRequest($"projects/{projectId}/projectfinancialstatus", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<ProjectFinancialStatus>>(request);
            return results.Data;
        }

        ///<summary>Deletes financial status for a project.</summary>
        public void DeleteFinancialStatus(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/projectfinancialstatus", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Project Financial Status

        #region Status

        ///<summary>Returns all the values from the project status valuelist.</summary>
        public IList<ProjectStatus> GetProjectStatus()
        {
            var request = _client.NewRequest("/projects/status");
            var results = _client.Execute<List<ProjectStatus>>(request);

            return results.Data;
        }

        ///<summary>Returns project status for a specific project.</summary>
        public ProjectStatus GetProjectStatus(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/status");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<ProjectStatus>(request);
            return results.Data;
        }

        ///<summary>Associates new or existing project status for a project.</summary>
        public IList<ProjectStatus> AddProjectStatusToProject(int projectId, string status)
        {
            //Data to post
            var data = new List<ProjectStatus>();

            //Look up project status
            var findRequest = _client.NewRequest("/projects/status");
            findRequest.AddQueryParameter("q", $"Name.raw:\"{status}\"");
            var findResults = _client.Execute<List<ProjectStatus>>(findRequest);

            if (findResults.Data.Any())
            {
                data.Add(findResults.Data.First());
            }
            else
            {
                //Add new project status
                var addRequest = _client.NewRequest("/projects/status", Method.POST);
                addRequest.AddBody(new List<ProjectStatus> { new ProjectStatus { Name = status } });
                var addResponse = _client.Execute<List<ProjectStatus>>(addRequest);
                if (addResponse.Data.Any()) data.Add(addResponse.Data.First());
                else throw new Exception($"Could not find or create a contract type named {status} in Cosential");
            }

            //Associate the project status to the project
            var request = _client.NewRequest($"projects/{projectId}/status", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<ProjectStatus>>(request);
            return results.Data;
        }

        ///<summary>Deletes status for a project.</summary>
        public void DeleteStatus(int projectId, string statusId)
        {
            var request = _client.NewRequest("projects/{projectId}/status/{statusId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("statusId", statusId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes status for a project.</summary>
        public void DeleteStatus(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/status", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Status

        #region Leed

        ///<summary>Returns leeds for all the projects.</summary>
        public IList<ProjectLeed> GetLeeds()
        {
            var request = _client.NewRequest("/projects/leed");
            var results = _client.Execute<List<ProjectLeed>>(request);

            return results.Data;
        }

        ///<summary>Returns leed for a specific project.</summary>
        public ProjectLeed GetLeed(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/leed");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<ProjectLeed>(request);
            return results.Data;
        }

        ///<summary>Associates new leed for a project.</summary>
        public ProjectLeed AddLeedToProject(int projectId, ProjectLeed leed)
        {
            var request = _client.NewRequest("projects/{projectId}/leed", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(leed);

            var results = _client.Execute<ProjectLeed>(request);
            return results.Data;
        }

        ///<summary>Deletes leed for a project.</summary>
        public void DeleteLeed(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/leed", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Leed

        #region Construction Schedule

        ///<summary>Returns construction schedule for all the projects.</summary>
        public IList<ProjectConstructionSchedule> GetConstructionSchedule()
        {
            var request = _client.NewRequest("/projects/constructionschedule");
            var results = _client.Execute<List<ProjectConstructionSchedule>>(request);

            return results.Data;
        }

        ///<summary>Returns construction schedule for a specific project.</summary>
        public ProjectConstructionSchedule GetConstructionSchedule(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/constructionschedule");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<ProjectConstructionSchedule>(request);
            return results.Data;
        }

        ///<summary>Associates new construction schedule for a project.</summary>
        public ProjectConstructionSchedule AddConstructionScheduleToProject(int projectId, ProjectConstructionSchedule constructionSchedule)
        {
            var request = _client.NewRequest("projects/{projectId}/constructionschedule", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(constructionSchedule);

            var results = _client.Execute<ProjectConstructionSchedule>(request);
            return results.Data;
        }

        ///<summary>Deletes construction schedule for a project.</summary>
        public void DeleteConstructionSchedule(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/constructionschedule", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(new ProjectConstructionSchedule { });
            _client.Execute(request);
        }

        #endregion Construction Schedule

        #region Rank

        ///<summary>Returns all the values from the project rank valuelist.</summary>
        public IList<ProjectRank> GetRanks()
        {
            var request = _client.NewRequest("/projects/projectrank");
            var results = _client.Execute<List<ProjectRank>>(request);

            return results.Data;
        }

        ///<summary>Returns rank for a specific project.</summary>
        public IList<ProjectRank> GetRank(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/projectrank");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectRank>>(request);
            return results.Data;
        }

        ///<summary>Returns specific rank for a project.</summary>
        public ProjectRank GetRank(int projectId, int rankId)
        {
            var request = _client.NewRequest("projects/{projectId}/projectrank/{rankId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("rankId", rankId.ToString());

            var results = _client.Execute<ProjectRank>(request);
            return results.Data;
        }

        ///<summary>Associates new or existing rank for a project.</summary>
        public IList<ProjectRank> AddRankToProject(int projectId, string rank)
        {
            //Data to post
            var data = new List<ProjectRank>();

            //Look up rank
            var findRequest = _client.NewRequest("/projects/projectrank");
            findRequest.AddQueryParameter("q", $"Name.raw:\"{rank}\"");
            var findResults = _client.Execute<List<ProjectRank>>(findRequest);

            if (findResults.Data.Any())
            {
                data.Add(findResults.Data.First());
            }
            else
            {
                //Add new rank
                var addRequest = _client.NewRequest("/projects/projectrank", Method.POST);
                addRequest.AddBody(new List<ProjectRank> { new ProjectRank { Name = rank } });
                var addResponse = _client.Execute<List<ProjectRank>>(addRequest);
                if (addResponse.Data.Any()) data.Add(addResponse.Data.First());
                else throw new Exception($"Could not find or create a contract type named {rank} in Cosential");
            }

            //Associate the rank to the project
            var request = _client.NewRequest($"projects/{projectId}/projectrank", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<ProjectRank>>(request);
            return results.Data;
        }

        ///<summary>Deletes rank for a project.</summary>
        public void DeleteRank(int projectId, string rankId)
        {
            var request = _client.NewRequest("projects/{projectId}/projectrank/{rankId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("rankId", rankId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the ranks for a project.</summary>
        public void DeleteRanks(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/projectrank", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Rank

        #region Publishable Reason

        ///<summary>Returns all the values from the project publishable reason valuelist.</summary>
        public IList<ProjectPublishableReason> GetPublishableReasons()
        {
            var request = _client.NewRequest("/projects/publishablereason");
            var results = _client.Execute<List<ProjectPublishableReason>>(request);

            return results.Data;
        }

        ///<summary>Returns publishable reasons for a specific project.</summary>
        public IList<ProjectPublishableReason> GetPublishableReasons(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/publishablereason");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectPublishableReason>>(request);
            return results.Data;
        }

        ///<summary>Returns specific publishable reason for a project.</summary>
        public ProjectPublishableReason GetPublishableReason(int projectId, int publishableReasonId)
        {
            var request = _client.NewRequest("projects/{projectId}/publishablereason/{publishableReasonId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("publishableReasonId", publishableReasonId.ToString());

            var results = _client.Execute<ProjectPublishableReason>(request);
            return results.Data;
        }

        ///<summary>Associates new or existing publishable reason for a project.</summary>
        public IList<ProjectPublishableReason> AddPublishableReasonToProject(int projectId, string reason)
        {
            //Data to post
            var data = new List<ProjectPublishableReason>();

            //Look up publishable reason
            var findRequest = _client.NewRequest("/projects/publishablereason");
            findRequest.AddQueryParameter("q", $"Name.raw:\"{reason}\"");
            var findResults = _client.Execute<List<ProjectPublishableReason>>(findRequest);

            if (findResults.Data.Any())
            {
                data.Add(findResults.Data.First());
            }
            else
            {
                //Add new publishable reason
                var addRequest = _client.NewRequest("/projects/publishablereason", Method.POST);
                addRequest.AddBody(new List<ProjectPublishableReason> { new ProjectPublishableReason { Name = reason } });
                var addResponse = _client.Execute<List<ProjectPublishableReason>>(addRequest);
                if (addResponse.Data.Any()) data.Add(addResponse.Data.First());
                else throw new Exception($"Could not find or create a contract type named {reason} in Cosential");
            }

            //Associate the publishable reason to the project
            var request = _client.NewRequest($"projects/{projectId}/publishablereason", Method.POST);
            request.AddBody(data);
            var results = _client.Execute<List<ProjectPublishableReason>>(request);
            return results.Data;
        }

        ///<summary>Deletes publishable reason for a project.</summary>
        public void DeletePublishableReason(int projectId, string publishableReasonId)
        {
            var request = _client.NewRequest("projects/{projectId}/publishablereason/{publishableReasonId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("publishableReasonId", publishableReasonId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the publishable reasons for a project.</summary>
        public void DeletePublishableReasons(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/publishablereason", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Publishable Reason

        #region AESchedule

        ///<summary>Returns ae schedule for a specific project.</summary>
        public ProjectAESchedule GetAESchedule(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/aeschedule");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<ProjectAESchedule>(request);
            return results.Data;
        }

        ///<summary>Associates new ae schedule for a project.</summary>
        public ProjectAESchedule AddAEScheduleToProject(int projectId, ProjectAESchedule aESchedule)
        {
            var request = _client.NewRequest("projects/{projectId}/aeschedule", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(aESchedule);

            var results = _client.Execute<ProjectAESchedule>(request);
            return results.Data;
        }

        #endregion AESchedule

        #region Consultant

        ///<summary>Returns consultants for a specific project.</summary>
        public IList<ProjectConsultant> GetConsultants(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/consultants");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectConsultant>>(request);
            return results.Data;
        }

        ///<summary>Returns specific consultant for a project.</summary>
        public ProjectConsultant GetConsultant(int projectId, int consultantId)
        {
            var request = _client.NewRequest("projects/{projectId}/consultants/{consultantId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("consultantId", consultantId.ToString());

            var results = _client.Execute<ProjectConsultant>(request);
            return results.Data;
        }

        ///<summary>Associates new consultants for a project.</summary>
        public IList<ProjectConsultant> AddConsultantsToProject(int projectId, List<ProjectConsultant> consultants)
        {
            var request = _client.NewRequest("projects/{projectId}/consultants", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(consultants);

            var results = _client.Execute<List<ProjectConsultant>>(request);
            return results.Data;
        }

        ///<summary>Deletes consultant for a project.</summary>
        public void DeleteConsultant(int projectId, string consultantId)
        {
            var request = _client.NewRequest("projects/{projectId}/consultants/{consultantId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("consultantId", consultantId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the consultants for a project.</summary>
        public void DeleteConsultants(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/consultants", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Consultant

        #region Consultant Contact

        ///<summary>Returns consultant contacts for a specific project.</summary>
        public IList<ProjectConsultantContact> GetConsultantContacts(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/consultantcontacts");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectConsultantContact>>(request);
            return results.Data;
        }

        ///<summary>Returns specific consultant contact for a project.</summary>
        public ProjectConsultantContact GetConsultantContact(int projectId, int consultantContactId)
        {
            var request = _client.NewRequest("projects/{projectId}/consultantcontacts/{consultantContactId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("consultantContactId", consultantContactId.ToString());

            var results = _client.Execute<ProjectConsultantContact>(request);
            return results.Data;
        }

        ///<summary>Associates new consultant contacts for a project.</summary>
        public IList<ProjectConsultantContact> AddConsultantContactsToProject(int projectId, List<ProjectConsultantContact> consultantContacts)
        {
            var request = _client.NewRequest("projects/{projectId}/consultantcontacts", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(consultantContacts);

            var results = _client.Execute<List<ProjectConsultantContact>>(request);
            return results.Data;
        }

        ///<summary>Deletes consultant contact for a project.</summary>
        public void DeleteConsultantContact(int projectId, string consultantContactId)
        {
            var request = _client.NewRequest("projects/{projectId}/consultantcontacts/{consultantContactId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("consultantContactId", consultantContactId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the consultant contacts for a project.</summary>
        public void DeleteConsultantContacts(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/consultantcontacts", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Consultant Contact

        #region Component

        ///<summary>Returns components for a specific project.</summary>
        public IList<ProjectComponent> GetComponents(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/components");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<ProjectComponent>>(request);
            return results.Data;
        }

        ///<summary>Returns specific component for a project.</summary>
        public ProjectComponent GetComponent(int projectId, int componentId)
        {
            var request = _client.NewRequest("projects/{projectId}/components/{componentId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("componentId", componentId.ToString());

            var results = _client.Execute<ProjectComponent>(request);
            return results.Data;
        }

        ///<summary>Associates new components for a project.</summary>
        public IList<ProjectComponent> AddComponentsToProject(int projectId, List<ProjectComponent> components)
        {
            var request = _client.NewRequest("projects/{projectId}/components", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(components);

            var results = _client.Execute<List<ProjectComponent>>(request);
            return results.Data;
        }

        ///<summary>Deletes component for a project.</summary>
        public void DeleteComponent(int projectId, string componentId)
        {
            var request = _client.NewRequest("projects/{projectId}/components/{componentId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("componentId", componentId.ToString());
            _client.Execute(request);
        }

        ///<summary>Deletes all the components for a project.</summary>
        public void DeleteComponents(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/components", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            _client.Execute(request);
        }

        #endregion Component

        #region Email      

        ///<summary>Returns emails for a specific project.</summary>
        public IList<Email> GetEmails(int projectId)
        {
            var request = _client.NewRequest("projects/{projectId}/emails");
            request.AddUrlSegment("projectId", projectId.ToString());

            var results = _client.Execute<List<Email>>(request);
            return results.Data;
        }

        ///<summary>Returns specific email for a project.</summary>
        public Email GetEmail(int projectId, int emailId)
        {
            var request = _client.NewRequest("projects/{projectId}/emails/{emailId}");
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("emailId", emailId.ToString());

            var results = _client.Execute<Email>(request);
            return results.Data;
        }

        ///<summary>Associates new emails for a project.</summary>
        public IList<Email> AddEmailsToProject(int projectId, List<Email> emails)
        {
            var request = _client.NewRequest("projects/{projectId}/emails", Method.POST);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddBody(emails);

            var results = _client.Execute<List<Email>>(request);
            return results.Data;
        }

        ///<summary>Deletes email for a project.</summary>
        public void DeleteEmail(int projectId, string emailId)
        {
            var request = _client.NewRequest("projects/{projectId}/emails/{emailId}", Method.DELETE);
            request.AddUrlSegment("projectId", projectId.ToString());
            request.AddUrlSegment("emailId", emailId.ToString());
            _client.Execute(request);
        }

        #endregion Email

        #endregion
    }
}