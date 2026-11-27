namespace GradientLabs.Api;

public readonly record struct CustomerSupportPlatformIdentifierType(string Value)
{
    public static readonly CustomerSupportPlatformIdentifierType IntercomLead = new("intercom_lead");
    public static readonly CustomerSupportPlatformIdentifierType IntercomUser = new("intercom_user");
    public static readonly CustomerSupportPlatformIdentifierType ZendeskConversationUser = new("zendesk_conversation_user");
    public static readonly CustomerSupportPlatformIdentifierType ZendeskSupportUser = new("zendesk_support_user");
    public static readonly CustomerSupportPlatformIdentifierType SalesforceContactId = new("salesforce_contact_id");
    public static readonly CustomerSupportPlatformIdentifierType SalesforceAccountId = new("salesforce_account_id");
    public static readonly CustomerSupportPlatformIdentifierType SalesforceContactEmail = new("salesforce_contact_email");
    public static readonly CustomerSupportPlatformIdentifierType SalesforceSuppliedEmail = new("salesforce_supplied_email");

    public override string ToString() => Value;
    public static implicit operator string(CustomerSupportPlatformIdentifierType v) => v.Value;
}
