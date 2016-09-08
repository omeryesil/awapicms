﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AWAPI.AWAPI_UserService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AWAPI_UserService.IUserService")]
    public interface IUserService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Get", ReplyAction="http://tempuri.org/IUserService/GetResponse")]
        System.Data.DataSet Get(long userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Login", ReplyAction="http://tempuri.org/IUserService/LoginResponse")]
        System.Data.DataSet Login(string email, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/GetList", ReplyAction="http://tempuri.org/IUserService/GetListResponse")]
        System.Data.DataSet GetList(string where, string sortField);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Delete", ReplyAction="http://tempuri.org/IUserService/DeleteResponse")]
        void Delete(long userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Add", ReplyAction="http://tempuri.org/IUserService/AddResponse")]
        long Add(string firstName, string lastName, string email, string password, string description, bool isEnabled, string link, string imageurl);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IUserService/Update", ReplyAction="http://tempuri.org/IUserService/UpdateResponse")]
        bool Update(long id, string firstName, string lastName, string email, string password, string description, bool isEnabled, string link, string imageurl);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUserServiceChannel : AWAPI.AWAPI_UserService.IUserService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserServiceClient : System.ServiceModel.ClientBase<AWAPI.AWAPI_UserService.IUserService>, AWAPI.AWAPI_UserService.IUserService {
        
        public UserServiceClient() {
        }
        
        public UserServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UserServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UserServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataSet Get(long userId) {
            return base.Channel.Get(userId);
        }
        
        public System.Data.DataSet Login(string email, string password) {
            return base.Channel.Login(email, password);
        }
        
        public System.Data.DataSet GetList(string where, string sortField) {
            return base.Channel.GetList(where, sortField);
        }
        
        public void Delete(long userId) {
            base.Channel.Delete(userId);
        }
        
        public long Add(string firstName, string lastName, string email, string password, string description, bool isEnabled, string link, string imageurl) {
            return base.Channel.Add(firstName, lastName, email, password, description, isEnabled, link, imageurl);
        }
        
        public bool Update(long id, string firstName, string lastName, string email, string password, string description, bool isEnabled, string link, string imageurl) {
            return base.Channel.Update(id, firstName, lastName, email, password, description, isEnabled, link, imageurl);
        }
    }
}