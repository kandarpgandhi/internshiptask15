create procedure sploginData
as
begin
select * from login
end
sploginData

create procedure spgetPurchase_TableData
as
begin
select * from Purchase_Table
end
spgetPurchase_TableData

create procedure spgetProductData
as
begin
select * from Purchase_Product
end
spgetProductData

create procedure spGetPurchaseId
as
begin
select Purchase_Id from Purchase_Table
end
spGetPurchaseId


create procedure spGetAllDataPurchaseId
@id int
as
begin
select * from Purchase_Product where Purchase_Id=@id
end
spGetAllDataPurchaseId 1

create procedure spgetdatabypurchaseId
@id int
as begin
select * from Purchase_Table where Purchase_Id=@id
end
spgetdatabypurchaseId 1

alter procedure sptwoTables
@id int
as
begin
select * from Purchase_Product where Purchase_Id=@id
select * from Purchase_Table where Purchase_Id=@id
end
sptwoTables 1

create procedure spGetTwoTableData
as
begin
select * from Purchase_Table
select * from Purchase_Product
end
spGetTwoTableData

alter procedure spGetDataInOneTable
as
begin
select Purchase_Table.Purchase_Id,Purchase_No,Date,Total,PurchaseProductId,Item,Quantity,Amount from Purchase_Table
inner join Purchase_Product
on Purchase_Table.Purchase_Id=Purchase_Product.Purchase_Id
end
spGetDataInOneTable