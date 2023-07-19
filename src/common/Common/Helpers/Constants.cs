namespace AsteriskDotHMG.Common.Helpers;

public static class Constants
{
    public const string EPPLUS_EXCEL_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string JSON_CONTENT_TYPE = "application/json";
    public const string XML_CONTENT_TYPE = "application/xml";
    public const string XML_TEXT_CONTENT_TYPE = "text/xml";
    public const string PDF_CONTENT_TYPE = "application/pdf";

    public const string LOGGING_ACTION_API_START = "[API START]";
    public const string LOGGING_ACTION_API_PROCESS = "[API PROCESS]";
    public const string LOGGING_ACTION_API_END = "[API END]";
    public const string LOGGING_ACTION_MEDIATR_START = "[MEDIATR START]";
    public const string LOGGING_ACTION_MEDIATR_PROCESS = "[MEDIATR PROCESS]";
    public const string LOGGING_ACTION_MEDIATR_END = "[MEDIATR END]";
    public const string LOGGING_ACTION_DATA = "[ACTION DATA]";
    public const string LOGGING_ACTION_GENERIC_ERROR = "[GENERIC ERROR]";
    public const string LOGGING_ACTION_VALIDATION_ERROR = "[VALIDATION ERROR]";
    public const string LOGGING_ACTION_IDENTITY_ERROR = "[IDENTITY ERROR]";
    public const string LOGGING_ACTION_INTEGRATION_ERROR = "[INTEGRATION ERROR]";
    public const string LOGGING_ACTION_TRIGGER_START = "[TRIGGER START]";
    public const string LOGGING_ACTION_TRIGGER_PROCESS = "[TRIGGER PROCESS]";
    public const string LOGGING_ACTION_TRIGGER_END = "[TRIGGER END]";
    public const string LOGGING_ACTION_TRIGGER_RESULT = "[TRIGGER RESULT]";
    public const string LOGGING_ACTION_TRIGGER_ERROR = "[TRIGGER ERROR]";
    public const string LOGGING_ACTION_TRIGGER_API_START = "[TRIGGER API START]";
    public const string LOGGING_ACTION_TRIGGER_API_PROCESS = "[TRIGGER API PROCESS]";
    public const string LOGGING_ACTION_TRIGGER_API_END = "[TRIGGER API END]";
    public const string LOGGING_ACTION_TRIGGER_API_ERROR = "[TRIGGER API ERROR]";
    public const string LOGGING_ACTION_THIRD_PARTY_API_ERROR = "[THIRD PARTY API ERROR]";
    public const string LOGGING_ACTION_EVENT_START = "[EVENT START]";
    public const string LOGGING_ACTION_EVENT_PROCESS = "[EVENT PROCESS]";
    public const string LOGGING_ACTION_EVENT_END = "[EVENT END]";
    public const string LOGGING_ACTION_EVENT_ERROR = "[EVENT ERROR]";
    public const string SENDGRID_HTML_CONTENT_VARIABLE = "htmlContent";
    public const string SENDGRID_STRING_CONTENT_VARIABLE = "stringContent";
}

public static class Messages
{
    //generic
    public const string INVALID_JSON_DATA = "Invalid JSON data";
    public const string INVALID_XML_DATA = "Invalid XML data";
    public const string INVALID_EXCEL_DATA = "Invalid Excel data";
    public const string INVALID_FILE_TYPE = "Invalid file type";   

    //blob
    public const string BLOB_NOT_FOUND = "The specified blob does not exists";
    public const string BLOB_DIRECTORY = "Location of the blob";
    public const string BLOB_FILE_NAME = "File name of the blob";
}