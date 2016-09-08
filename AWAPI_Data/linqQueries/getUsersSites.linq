<Query Kind="Expression">
  <Connection>
    <ID>55b31074-6fec-4e77-9c54-4bbdbeff157f</ID>
    <Server>omer-pc</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>AWAPI</Database>
    <UserName>sa</UserName>
  </Connection>
</Query>

from st in AwSites
join stusr in AwSiteUsers on st.SiteId equals stusr.SiteId
where stusr.UserId.Equals(2)
orderby st.Title
select new
{
	st.SiteId, 
	st.Title, 
	st.Description,
	st.Link, 
	st.Imageurl, 
	st.PubDate,	
	st.OwnerUserId, 
	st.Status, 
	st.LastBuildDate, 
	stusr.UserType,	
	stusr.AwUser.FirstName,	
	stusr.AwUser.LastName, 
	stusr.AwUser.Email
}


from fl in AwFiles
group fl by fl.ContentType into g 
select g
