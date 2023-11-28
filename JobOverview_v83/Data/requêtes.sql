delete from Releases where CodeLogiciel = 'ANATOMIA' and NumeroVersion = 6

select * from Releases where CodeLogiciel = 'ANATOMIA' and NumeroVersion = 6

-- Nombre de travaux par tâche
select t.Id, t.Personne, t.CodeLogiciel, t.NumVersion, count(w.DateTravail)
from Taches t
left outer join Travaux w on t.Id = w.IdTache
group by t.Id, t.Personne, t.CodeLogiciel, t.NumVersion

declare @id int = 44
delete from Travaux where IdTache = @id
select @id = max(Id) from Taches
DBCC CHECKIDENT ('Taches', RESEED, @id) 

select * from Taches

