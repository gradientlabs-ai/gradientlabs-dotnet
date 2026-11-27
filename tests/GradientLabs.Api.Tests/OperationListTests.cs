using System.Reflection;
using FluentAssertions;
using Xunit;

namespace GradientLabs.Api.Tests;

// Each public API contract operation this library implements, keyed by the contract's operation id.
// The API sync updates this map alongside PUBLIC_API_VERSION. GET /spec.json is left out on purpose.
public class OperationListTests
{
    private static readonly IReadOnlyDictionary<string, (Type Client, string Method)> Operations = new Dictionary<string, (Type, string)>
    {
        ["ArticleDelete"] = (typeof(ArticlesClient), nameof(ArticlesClient.DeleteAsync)),
        ["ArticleSetUsageStatus"] = (typeof(ArticlesClient), nameof(ArticlesClient.SetUsageStatusAsync)),
        ["ArticleTopicUpsert"] = (typeof(TopicsClient), nameof(TopicsClient.UpsertArticleTopicAsync)),
        ["ArticleUpsert"] = (typeof(ArticlesClient), nameof(ArticlesClient.UpsertAsync)),
        ["BackOfficeTaskCreate"] = (typeof(BackOfficeTasksClient), nameof(BackOfficeTasksClient.CreateAsync)),
        ["BackOfficeTaskDelete"] = (typeof(BackOfficeTasksClient), nameof(BackOfficeTasksClient.DeleteAsync)),
        ["BackOfficeTaskRead"] = (typeof(BackOfficeTasksClient), nameof(BackOfficeTasksClient.ReadAsync)),
        ["ConversationAddMessage"] = (typeof(ConversationsClient), nameof(ConversationsClient.AddMessageAsync)),
        ["ConversationAssign"] = (typeof(ConversationsClient), nameof(ConversationsClient.AssignAsync)),
        ["ConversationCancel"] = (typeof(ConversationsClient), nameof(ConversationsClient.CancelAsync)),
        ["ConversationDelete"] = (typeof(ConversationsClient), nameof(ConversationsClient.DeleteAsync)),
        ["ConversationEvent"] = (typeof(ConversationsClient), nameof(ConversationsClient.AddEventAsync)),
        ["ConversationFinish"] = (typeof(ConversationsClient), nameof(ConversationsClient.FinishAsync)),
        ["ConversationRate"] = (typeof(ConversationsClient), nameof(ConversationsClient.RateAsync)),
        ["ConversationRead"] = (typeof(ConversationsClient), nameof(ConversationsClient.ReadAsync)),
#pragma warning disable CS0618
        ["ConversationReadDeprecated"] = (typeof(ConversationsClient), nameof(ConversationsClient.ReadDeprecatedAsync)),
        ["CustomerMemoriesBatchCreate"] = (typeof(CustomersClient), nameof(CustomersClient.BatchCreateMemoriesDeprecatedAsync)),
#pragma warning restore CS0618
        ["ConversationResume"] = (typeof(ConversationsClient), nameof(ConversationsClient.ResumeAsync)),
        ["ConversationReturnAsyncToolResult"] = (typeof(ConversationsClient), nameof(ConversationsClient.ReturnAsyncToolResultAsync)),
        ["ConversationStart"] = (typeof(ConversationsClient), nameof(ConversationsClient.StartAsync)),
        ["CustomerMemoriesBatchCreateV2"] = (typeof(CustomersClient), nameof(CustomersClient.BatchCreateMemoriesAsync)),
        ["CustomerMemoriesList"] = (typeof(CustomersClient), nameof(CustomersClient.ListMemoriesAsync)),
        ["FileCreate"] = (typeof(FilesClient), nameof(FilesClient.CreateAsync)),
        ["FileList"] = (typeof(FilesClient), nameof(FilesClient.ListAsync)),
        ["FileUploadBegin"] = (typeof(FilesClient), nameof(FilesClient.BeginUploadAsync)),
        ["GetDefaultHandOffTarget"] = (typeof(HandOffTargetsClient), nameof(HandOffTargetsClient.GetDefaultAsync)),
        ["HandOffTargetDelete"] = (typeof(HandOffTargetsClient), nameof(HandOffTargetsClient.DeleteAsync)),
        ["HandOffTargetUpsert"] = (typeof(HandOffTargetsClient), nameof(HandOffTargetsClient.UpsertAsync)),
        ["IPAddressList"] = (typeof(IpAddressesClient), nameof(IpAddressesClient.ListAsync)),
        ["KnowledgeBaseCreate"] = (typeof(KnowledgeBasesClient), nameof(KnowledgeBasesClient.CreateAsync)),
        ["KnowledgeBaseList"] = (typeof(KnowledgeBasesClient), nameof(KnowledgeBasesClient.ListAsync)),
        ["KnowledgeBaseRead"] = (typeof(KnowledgeBasesClient), nameof(KnowledgeBasesClient.ReadAsync)),
        ["KnowledgeBaseUpdate"] = (typeof(KnowledgeBasesClient), nameof(KnowledgeBasesClient.UpdateAsync)),
        ["ListHandOffTargets"] = (typeof(HandOffTargetsClient), nameof(HandOffTargetsClient.ListAsync)),
        ["ListProcedures"] = (typeof(ProceduresClient), nameof(ProceduresClient.ListAsync)),
        ["ListTopics"] = (typeof(TopicsClient), nameof(TopicsClient.ListAsync)),
        ["ListTrafficGroups"] = (typeof(TrafficGroupsClient), nameof(TrafficGroupsClient.ListAsync)),
        ["NoteCreate"] = (typeof(NotesClient), nameof(NotesClient.CreateAsync)),
        ["NoteDelete"] = (typeof(NotesClient), nameof(NotesClient.DeleteAsync)),
        ["NoteList"] = (typeof(NotesClient), nameof(NotesClient.ListAsync)),
        ["NoteRead"] = (typeof(NotesClient), nameof(NotesClient.ReadAsync)),
        ["NoteSetStatus"] = (typeof(NotesClient), nameof(NotesClient.SetStatusAsync)),
        ["NoteUpdate"] = (typeof(NotesClient), nameof(NotesClient.UpdateAsync)),
        ["OutboundChatConversationStart"] = (typeof(OutboundClient), nameof(OutboundClient.StartChatConversationAsync)),
        ["OutboundEmailConversationStart"] = (typeof(OutboundClient), nameof(OutboundClient.StartEmailConversationAsync)),
        ["OutboundPhoneConversationStart"] = (typeof(OutboundClient), nameof(OutboundClient.StartPhoneConversationAsync)),
        ["ProcedureListVersions"] = (typeof(ProceduresClient), nameof(ProceduresClient.ListVersionsAsync)),
        ["ProcedureSetGatedVersion"] = (typeof(ProceduresClient), nameof(ProceduresClient.SetGatedAsync)),
        ["ProcedureSetLiveVersion"] = (typeof(ProceduresClient), nameof(ProceduresClient.SetLiveAsync)),
        ["ProcedureUnsetGatedVersion"] = (typeof(ProceduresClient), nameof(ProceduresClient.UnsetGatedAsync)),
        ["ProcedureUnsetLiveVersion"] = (typeof(ProceduresClient), nameof(ProceduresClient.UnsetLiveAsync)),
        ["ReadProcedure"] = (typeof(ProceduresClient), nameof(ProceduresClient.ReadAsync)),
        ["ReadTopic"] = (typeof(TopicsClient), nameof(TopicsClient.ReadAsync)),
        ["ResourceSourceCreate"] = (typeof(ResourceSourcesClient), nameof(ResourceSourcesClient.CreateAsync)),
        ["ResourceSourceDelete"] = (typeof(ResourceSourcesClient), nameof(ResourceSourcesClient.DeleteAsync)),
        ["ResourceSourceList"] = (typeof(ResourceSourcesClient), nameof(ResourceSourcesClient.ListAsync)),
        ["ResourceSourceRead"] = (typeof(ResourceSourcesClient), nameof(ResourceSourcesClient.ReadAsync)),
        ["ResourceSourceUpdate"] = (typeof(ResourceSourcesClient), nameof(ResourceSourcesClient.UpdateAsync)),
        ["ResourceTypeCreate"] = (typeof(ResourceTypesClient), nameof(ResourceTypesClient.CreateAsync)),
        ["ResourceTypeDelete"] = (typeof(ResourceTypesClient), nameof(ResourceTypesClient.DeleteAsync)),
        ["ResourceTypeList"] = (typeof(ResourceTypesClient), nameof(ResourceTypesClient.ListAsync)),
        ["ResourceTypeRead"] = (typeof(ResourceTypesClient), nameof(ResourceTypesClient.ReadAsync)),
        ["ResourceTypeUpdate"] = (typeof(ResourceTypesClient), nameof(ResourceTypesClient.UpdateAsync)),
        ["SecretList"] = (typeof(SecretsClient), nameof(SecretsClient.ListAsync)),
        ["SecretRevoke"] = (typeof(SecretsClient), nameof(SecretsClient.RevokeAsync)),
        ["SecretWrite"] = (typeof(SecretsClient), nameof(SecretsClient.WriteAsync)),
        ["SetDefaultHandOffTarget"] = (typeof(HandOffTargetsClient), nameof(HandOffTargetsClient.SetDefaultAsync)),
        ["SetProcedureLimit"] = (typeof(ProceduresClient), nameof(ProceduresClient.SetLimitAsync)),
        ["TerminologySubstitutionCreate"] = (typeof(TerminologySubstitutionsClient), nameof(TerminologySubstitutionsClient.CreateAsync)),
        ["TerminologySubstitutionDelete"] = (typeof(TerminologySubstitutionsClient), nameof(TerminologySubstitutionsClient.DeleteAsync)),
        ["TerminologySubstitutionList"] = (typeof(TerminologySubstitutionsClient), nameof(TerminologySubstitutionsClient.ListAsync)),
        ["TerminologySubstitutionRead"] = (typeof(TerminologySubstitutionsClient), nameof(TerminologySubstitutionsClient.ReadAsync)),
        ["TerminologySubstitutionUpdate"] = (typeof(TerminologySubstitutionsClient), nameof(TerminologySubstitutionsClient.UpdateAsync)),
        ["ToolCreate"] = (typeof(ToolsClient), nameof(ToolsClient.CreateAsync)),
        ["ToolDelete"] = (typeof(ToolsClient), nameof(ToolsClient.DeleteAsync)),
        ["ToolExecute"] = (typeof(ToolsClient), nameof(ToolsClient.ExecuteAsync)),
        ["ToolList"] = (typeof(ToolsClient), nameof(ToolsClient.ListAsync)),
        ["ToolRead"] = (typeof(ToolsClient), nameof(ToolsClient.ReadAsync)),
        ["ToolUpdate"] = (typeof(ToolsClient), nameof(ToolsClient.UpdateAsync)),
        ["TrafficGroupCreate"] = (typeof(TrafficGroupsClient), nameof(TrafficGroupsClient.CreateAsync)),
        ["TrafficGroupDelete"] = (typeof(TrafficGroupsClient), nameof(TrafficGroupsClient.DeleteAsync)),
        ["TrafficGroupExclusionCreate"] = (typeof(TrafficGroupsClient), nameof(TrafficGroupsClient.CreateExclusionAsync)),
        ["TrafficGroupExclusionDelete"] = (typeof(TrafficGroupsClient), nameof(TrafficGroupsClient.DeleteExclusionAsync)),
        ["TrafficGroupTargetCreate"] = (typeof(TrafficGroupsClient), nameof(TrafficGroupsClient.CreateTargetAsync)),
        ["TrafficGroupTargetDelete"] = (typeof(TrafficGroupsClient), nameof(TrafficGroupsClient.DeleteTargetAsync)),
        ["TrafficGroupUpdate"] = (typeof(TrafficGroupsClient), nameof(TrafficGroupsClient.UpdateAsync)),
        ["UpdateSchemaByExamples"] = (typeof(ResourceSourcesClient), nameof(ResourceSourcesClient.UpdateSchemaByExamplesAsync)),
        ["VoiceCallContextRead"] = (typeof(VoiceClient), nameof(VoiceClient.ReadLatestCallContextAsync)),
    };

    public static TheoryData<string> OperationIds()
    {
        var data = new TheoryData<string>();
        foreach (var id in Operations.Keys)
            data.Add(id);
        return data;
    }

    [Theory]
    [MemberData(nameof(OperationIds))]
    public void ListedOperation_HasAPublicMethod(string operationId)
    {
        var (client, method) = Operations[operationId];

        client.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Should().Contain(m => m.Name == method, $"{operationId} maps to {client.Name}.{method}");
    }

    [Fact]
    public void EveryClientGroup_IsReachableFromARootClient()
    {
        var groups = typeof(GradientLabsIntegrationClient).GetProperties()
            .Concat(typeof(GradientLabsManagementClient).GetProperties())
            .Select(p => p.PropertyType)
            .ToHashSet();

        Operations.Values.Select(o => o.Client).Distinct().Should().OnlyContain(t => groups.Contains(t));
    }
}
