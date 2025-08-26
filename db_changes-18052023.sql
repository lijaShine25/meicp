
CREATE TABLE [dbo].[parts_documents](
	[part_doc_slno] [int] IDENTITY(1,1) NOT NULL,
	[part_slno] [int] NOT NULL,
	[doc_title] [varchar](100) NULL,
	[doc_filename] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[part_doc_slno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[parts_documents]  WITH CHECK ADD  CONSTRAINT [fk_partdocument_parts] FOREIGN KEY([part_slno])
REFERENCES [dbo].[parts] ([part_slno])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[parts_documents] CHECK CONSTRAINT [fk_partdocument_parts]
GO


insert into parts_documents (part_slno, doc_filename, doc_title) 
select part_slno,uploadfile1,left(uploadfile1,CHARINDEX('.pdf',uploadfile1))
from parts
where uploadfile1!='' and uploadfile1 is not null;

insert into parts_documents (part_slno, doc_filename, doc_title) 
select part_slno,uploadfile2,left(uploadfile2,CHARINDEX('.pdf',uploadfile2))
from parts
where uploadfile2!='' and uploadfile2 is not null;

insert into parts_documents (part_slno, doc_filename, doc_title) 
select part_slno,uploadfile3,left(uploadfile3,CHARINDEX('.pdf',uploadfile3))
from parts
where uploadfile3!='' and uploadfile3 is not null;

insert into parts_documents (part_slno, doc_filename, doc_title) 
select part_slno,uploadfile4,left(uploadfile4,CHARINDEX('.pdf',uploadfile4))
from parts
where uploadfile4!='' and uploadfile4 is not null;

update parts_documents set doc_title=left(doc_filename,CHARINDEX('.tif',doc_filename))
where doc_title='';

alter table temp_rptcontrolplan add 
method_slno int, machine_slno int, 
freq_slno int,evalTech_slno int, 
evaltech varchar(100);

alter table samplefrequency add 
foi bit not null default 1, 
pcc bit not null default 1, 
material_test_report bit not null default 1,
pmc bit not null default 1;

update SampleFrequency set foi=1;

-- rename sp_temp_controlplan to sp_temp_controlplan2

USE [mstl_controlplan]
GO
/****** Object:  StoredProcedure [dbo].[SP_Temp_RptControlPlan]    Script Date: 21-05-2023 06:39:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[SP_Temp_RptControlPlan](@cp_slno INT=NULL,
@part_slno INT=NULL,
@oper_slno INT=NULL)
AS BEGIN
    DELETE FROM Temp_RptControlPlan
	
INSERT INTO Temp_RptControlPlan([cp_slno], [mstPartNo], [partIssueNo], [partIssueDt], [keyContact], [keyContactPhone],
[organization], [orgApprovalDt], [orgDate], [originalDt], [rev_no], 
[Date], [customerPartNo], [customerIssueNo], 
[customerIssueDt],  [custApproval], [custApprovalDt], 
[Customer_name], [PartDescription], [custQaApproval], [custQaApprovalDt], 
[otherApproval], [otherApprovalDt], 
[dimn_no], [product_char], [process_char], [splChar_slno], [tol_min], [tol_max], 
 [gaugeCode], [sampleSize],  [res], [reactionPlan], [rev_date], cpType, operation_slno,
 operationdesc,machinedesc,method_slno, CftTeamSlNo, machine_slno, freq_slno, evalTech_slno)

SELECT B.cp_slno, A.mstPartNo, A.partIssueNo, A.partIssueDt, A.keyContact, A.keyContactPhone, 
A.organization, A.orgApprovalDt, A.orgDate, A.originalDt, C.user_revNo, 
convert(VARCHAR(10), getdate(), 103), A.customerPartNo, A.customerIssueNo, 
A.customerIssueDt, A.custApproval, A.custApprovalDt, 
A.Customer_name, A.PartDescription, A.custQaApproval, A.custQaApprovalDt, 
A.otherApproval, A.otherApprovalDt, 
B.dimn_no, B.product_char, B.process_char, B.splChar_slno, B.tol_min, B.tol_max, 
B.gaugeCode, B.sampleSize, B.res,  B.reactionPlan, 
C.user_revDt, A.cpType, c.operation_slno, 
'' as operation_desc,'' as machinedesc, b.method_slno,A.CftTeamSlNo,c.machine_slno,
b.freq_slno, b.evalTech_slno
    FROM ControlPlan_Child B
         inner join ControlPlan C ON B.cp_slno=C.cp_slno
         inner join Parts A ON C.part_slno=A.part_slno
    WHERE(C.cp_slno=B.cp_slno)AND C.part_slno=@part_slno AND c.operation_slno=@oper_slno AND c.cp_slno=@cp_slno


-- update the process no from parts mapping
update Temp_RptControlPlan
set process_no=(select process_no from PartsMapping pm 
where pm.part_slno=@part_slno 
and Temp_RptControlPlan.operation_slno=pm.operation_slno);

-- udpate method desc
update Temp_RptControlPlan
set methodDesc=(select methodDesc
from ControlMethods
where method_slno=Temp_RptControlPlan.method_slno);

---- update cft team name
update Temp_RptControlPlan
set CFTeamName = (select c.cfteamname from CFTeams c 
where c.CFTeamSlNo=Temp_RptControlPlan.CftTeamSlNo )

-- update machine desc
update Temp_RptControlPlan
set MachineDesc=(select MachineDesc
from machines m where m.machine_slno=Temp_RptControlPlan.machine_slno);

-- update operation desc
update Temp_RptControlPlan
set OperationDesc=(select OperationDesc 
from operations o 
where o.operation_slno=Temp_RptControlPlan.operation_slno);

-- update freq desc
update Temp_RptControlPlan
set sampleFreq=(select s.freqdesc 
from SampleFrequency s
where s.freq_slno=Temp_RptControlPlan.freq_slno);

-- update eval tech
update Temp_RptControlPlan
set measurementTech=(select e.evaltech
from EvaluationTech e 
where e.evalTech_slno=Temp_RptControlPlan.evaltech_slno);

-- prepared by name
update Temp_RptControlPlan
set preparedBy=(select distinct e.EmployeeName
from Employees e
inner join ControlPlan cp on cp.preparedBy=e.EmployeeSlNo
where cp.cp_slno=Temp_RptControlPlan.cp_slno);

-- approved by
update Temp_RptControlPlan
set approvedBy=(select distinct e.EmployeeName
from Employees e
inner join ControlPlan cp on cp.approvedBy=e.EmployeeSlNo
where cp.cp_slno=Temp_RptControlPlan.cp_slno);

    ---- update the team members
    DECLARE @cftSlNo INT
    SELECT @cftSlNo=CFTeamSlNo
    FROM CFTeams
    WHERE CFTeamName=(SELECT DISTINCT cfteamname FROM Temp_RptControlPlan)
    UPDATE Temp_RptControlPlan
    SET CFTeamName=(SELECT DISTINCT stuff((SELECT ', '+emp.EmployeeName
                                           FROM cfteamemployees tm
                                                INNER JOIN Employees emp ON emp.employeeslno=tm.employeeslno
                                           WHERE cfteamslno=@cftSlNo
                                           ORDER BY employeename
                                          FOR XML path('')), 1, 1, '') AS teamMembers)
    DELETE FROM temp_rptControlPlan WHERE product_char IS NULL;
END



