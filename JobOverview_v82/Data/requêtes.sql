delete from Releases where CodeLogiciel = 'ANATOMIA' and NumeroVersion = 6

select * from Releases where CodeLogiciel = 'ANATOMIA' and NumeroVersion = 6

-- Nombre de travaux par tâche
select t.Id, t.Personne, t.CodeLogiciel, t.NumVersion, count(w.DateTravail)
from Taches t
left outer join Travaux w on t.Id = w.IdTache
group by t.Id, t.Personne, t.CodeLogiciel, t.NumVersion

delete from Travaux where IdTache = 43