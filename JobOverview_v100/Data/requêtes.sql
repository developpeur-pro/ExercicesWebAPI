delete from Releases where CodeLogiciel = 'ANATOMIA' and NumeroVersion = 6

select * from Releases where CodeLogiciel = 'ANATOMIA' and NumeroVersion = 6

-- Nombre de travaux par tâche
select t.Id, t.Personne, t.CodeLogiciel, t.NumVersion, count(w.DateTravail)
from Taches t
left outer join Travaux w on t.Id = w.IdTache
group by t.Id, t.Personne, t.CodeLogiciel, t.NumVersion

declare @id int
delete from Travaux where IdTache > 44
delete from Taches where Id > 44
select @id = max(Id) from Taches
DBCC CHECKIDENT ('Taches', RESEED, @id) 


select * from logiciels l
inner join versions v on l.Code = v.CodeLogiciel

select * from logiciels l
inner join modules m on l.Code = m.CodeLogiciel

select * from Equipes e 
inner join Personnes p on e.Code = p.CodeEquipe

select * from Taches t
left outer join Travaux tr on t.Id = tr.IdTache
where t.CodeLogiciel = 'ANATOMIA'

