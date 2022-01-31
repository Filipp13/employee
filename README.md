##Сервис Employee

###создание контекстов для INFRA

```
Scaffold-DbContext "Data Source=RUMOS0255.ATREMA.DELOITTE.COM;Initial Catalog=People;" Microsoft.EntityFrameworkCore.SqlServer -t vPersonDepAndBU
Scaffold-DbContext "Data Source=RUMOS0929.ATREMA.DELOITTE.COM;Initial Catalog=PracticeManagementDEV;" Microsoft.EntityFrameworkCore.SqlServer -t Employee
```