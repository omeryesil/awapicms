﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AWAPI.AWAPI_ContentService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AWAPI_ContentService.IContentService")]
    public interface IContentService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/Get", ReplyAction="http://tempuri.org/IContentService/GetResponse")]
        AWAPI_Data.CustomEntities.ContentExtended Get(long contentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/GetList", ReplyAction="http://tempuri.org/IContentService/GetListResponse")]
        AWAPI_Data.CustomEntities.ContentExtended[] GetList(long siteId, string where, string sortField);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/Delete", ReplyAction="http://tempuri.org/IContentService/DeleteResponse")]
        void Delete(long contentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/Add", ReplyAction="http://tempuri.org/IContentService/AddResponse")]
        long Add(
                    string alias, 
                    string title, 
                    string description, 
                    string contentType, 
                    long siteId, 
                    long userId, 
                    long parentContentId, 
                    System.Nullable<System.DateTime> eventStartDate, 
                    System.Nullable<System.DateTime> endDate, 
                    string link, 
                    string imageurl, 
                    int sortOrder, 
                    bool isEnabled, 
                    bool isCommentable, 
                    System.Nullable<System.DateTime> pubDate, 
                    System.Nullable<System.DateTime> pubEndDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/Update", ReplyAction="http://tempuri.org/IContentService/UpdateResponse")]
        bool Update(
                    long id, 
                    string alias, 
                    string title, 
                    string description, 
                    string contentType, 
                    long userId, 
                    long parentContentId, 
                    System.Nullable<System.DateTime> eventStartDate, 
                    System.Nullable<System.DateTime> endDate, 
                    string link, 
                    string imageurl, 
                    int sortOrder, 
                    bool isEnabled, 
                    bool isCommentable, 
                    System.Nullable<System.DateTime> pubDate, 
                    System.Nullable<System.DateTime> pubEndDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/GetField", ReplyAction="http://tempuri.org/IContentService/GetFieldResponse")]
        AWAPI_Data.Data.awContentCustomField GetField(long fieldId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/GetFieldList", ReplyAction="http://tempuri.org/IContentService/GetFieldListResponse")]
        AWAPI_Data.CustomEntities.ContentCustomField[] GetFieldList(long contentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/SaveField", ReplyAction="http://tempuri.org/IContentService/SaveFieldResponse")]
        long SaveField(AWAPI_Data.Data.awContentCustomField fld);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/DeleteField", ReplyAction="http://tempuri.org/IContentService/DeleteFieldResponse")]
        void DeleteField(int fieldId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/DeleteFieldsByContentId", ReplyAction="http://tempuri.org/IContentService/DeleteFieldsByContentIdResponse")]
        void DeleteFieldsByContentId(long contentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/GetFieldValue", ReplyAction="http://tempuri.org/IContentService/GetFieldValueResponse")]
        AWAPI_Data.Data.awContentCustomFieldValue GetFieldValue(int valueId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/GetFieldValueByContentId", ReplyAction="http://tempuri.org/IContentService/GetFieldValueByContentIdResponse")]
        AWAPI_Data.Data.awContentCustomFieldValue GetFieldValueByContentId(long contentId, int fieldId, string cultureCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/GetFieldValueList", ReplyAction="http://tempuri.org/IContentService/GetFieldValueListResponse")]
        AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended[] GetFieldValueList(long contentId, string cultureCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/UpdateFieldValue", ReplyAction="http://tempuri.org/IContentService/UpdateFieldValueResponse")]
        long UpdateFieldValue(long contentId, int fieldId, long userId, string value, string cultureCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/DeleteFieldValue", ReplyAction="http://tempuri.org/IContentService/DeleteFieldValueResponse")]
        void DeleteFieldValue(int valueId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/DeleteFieldValues", ReplyAction="http://tempuri.org/IContentService/DeleteFieldValuesResponse")]
        void DeleteFieldValues(int fieldId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IContentService/DeleteFieldValuesByContentId", ReplyAction="http://tempuri.org/IContentService/DeleteFieldValuesByContentIdResponse")]
        void DeleteFieldValuesByContentId(long contentId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IContentServiceChannel : AWAPI.AWAPI_ContentService.IContentService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ContentServiceClient : System.ServiceModel.ClientBase<AWAPI.AWAPI_ContentService.IContentService>, AWAPI.AWAPI_ContentService.IContentService {
        
        public ContentServiceClient() {
        }
        
        public ContentServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ContentServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ContentServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ContentServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public AWAPI_Data.CustomEntities.ContentExtended Get(long contentId) {
            return base.Channel.Get(contentId);
        }
        
        public AWAPI_Data.CustomEntities.ContentExtended[] GetList(long siteId, string where, string sortField) {
            return base.Channel.GetList(siteId, where, sortField);
        }
        
        public void Delete(long contentId) {
            base.Channel.Delete(contentId);
        }
        
        public long Add(
                    string alias, 
                    string title, 
                    string description, 
                    string contentType, 
                    long siteId, 
                    long userId, 
                    long parentContentId, 
                    System.Nullable<System.DateTime> eventStartDate, 
                    System.Nullable<System.DateTime> endDate, 
                    string link, 
                    string imageurl, 
                    int sortOrder, 
                    bool isEnabled, 
                    bool isCommentable, 
                    System.Nullable<System.DateTime> pubDate, 
                    System.Nullable<System.DateTime> pubEndDate) {
            return base.Channel.Add(alias, title, description, contentType, siteId, userId, parentContentId, eventStartDate, endDate, link, imageurl, sortOrder, isEnabled, isCommentable, pubDate, pubEndDate);
        }
        
        public bool Update(
                    long id, 
                    string alias, 
                    string title, 
                    string description, 
                    string contentType, 
                    long userId, 
                    long parentContentId, 
                    System.Nullable<System.DateTime> eventStartDate, 
                    System.Nullable<System.DateTime> endDate, 
                    string link, 
                    string imageurl, 
                    int sortOrder, 
                    bool isEnabled, 
                    bool isCommentable, 
                    System.Nullable<System.DateTime> pubDate, 
                    System.Nullable<System.DateTime> pubEndDate) {
            return base.Channel.Update(id, alias, title, description, contentType, userId, parentContentId, eventStartDate, endDate, link, imageurl, sortOrder, isEnabled, isCommentable, pubDate, pubEndDate);
        }
        
        public AWAPI_Data.Data.awContentCustomField GetField(long fieldId) {
            return base.Channel.GetField(fieldId);
        }
        
        public AWAPI_Data.CustomEntities.ContentCustomField[] GetFieldList(long contentId) {
            return base.Channel.GetFieldList(contentId);
        }
        
        public long SaveField(AWAPI_Data.Data.awContentCustomField fld) {
            return base.Channel.SaveField(fld);
        }
        
        public void DeleteField(int fieldId) {
            base.Channel.DeleteField(fieldId);
        }
        
        public void DeleteFieldsByContentId(long contentId) {
            base.Channel.DeleteFieldsByContentId(contentId);
        }
        
        public AWAPI_Data.Data.awContentCustomFieldValue GetFieldValue(int valueId) {
            return base.Channel.GetFieldValue(valueId);
        }
        
        public AWAPI_Data.Data.awContentCustomFieldValue GetFieldValueByContentId(long contentId, int fieldId, string cultureCode) {
            return base.Channel.GetFieldValueByContentId(contentId, fieldId, cultureCode);
        }
        
        public AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended[] GetFieldValueList(long contentId, string cultureCode) {
            return base.Channel.GetFieldValueList(contentId, cultureCode);
        }
        
        public long UpdateFieldValue(long contentId, int fieldId, long userId, string value, string cultureCode) {
            return base.Channel.UpdateFieldValue(contentId, fieldId, userId, value, cultureCode);
        }
        
        public void DeleteFieldValue(int valueId) {
            base.Channel.DeleteFieldValue(valueId);
        }
        
        public void DeleteFieldValues(int fieldId) {
            base.Channel.DeleteFieldValues(fieldId);
        }
        
        public void DeleteFieldValuesByContentId(long contentId) {
            base.Channel.DeleteFieldValuesByContentId(contentId);
        }
    }
}