 Insert into CityListings (ClientId,defaultCity,lockcountyPicker)
 select Clientid, replace(community_placeholder,'e.g. ',''), 1 from ClientBoundaries where 
 ClientName='city of Mesa'
 
