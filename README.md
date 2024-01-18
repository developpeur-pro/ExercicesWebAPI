# ExercicesWebAPI

### v35
Création du projet et du modèle EF (partie Logiciels) sans les relations entre entités

### v37
Ajout des relations entre entités dans le modèle EF créé précédemment

### v40
Création d'un jeu de données par le code (appliqué par migration). Ajout d'un script SQL à exécuter sur la base pour compléter le jeu de données.

### v55 Requêtes GET
Création d'un contrôleur avec actions de lecture pour les logiciels

### v59 Exo de synthèse
Ajout des tables Equipes, Services, Personnes et Metiers dans la baes.  
Ajout d'un petit jeu de données.
Création d'un contrôleur pour exploiter ces tables.

### v67 Requêtes POST
Création d'actions pour l'ajout de releases au format FormData et d'équipes et personnes au format JSON

### v75 Gestion des erreurs et journalisation
Gestion des erreurs liées aux contraintes d'intégrité pour renvoyer des réponses 400.   
Journalisation détaillée de ces erreurs avec la méthode d'extension du contrôleur vue dans le cours.

### v77 Validation
Ajout de règles de validation implémentées manuellement sur les versions et releases.

### v81 Complétion du modèle
Ajout des tables Activites, ActivitesMetiers, Taches et Travaux.  
Insertion d'un jeu de données.

### v82 Création du contrôleur Taches
Création du contrôleur avec actions de lecture et d'ajout pour les tâches et travaux.

### v83 Ajout de règles métier
Ajout de règles métier pour la création de tâches et travaux.

### v94 Requêtes DELETE, PUT et PATCH
Ajout d'actions DELETE et PUT pour la suppression et la modification de tâches, travaux et personnes.  
Ajout d'une action PATCH pour la modification de versions.

### v100 Authentification
Création d'un serveur d'identité avec Duende IdentityServer  
Sécurisation de l'API et création d'une appli cliente de test Blazor WASM avec Backend For Frontend

### v114 Autorisation
Ajout d'utilisateurs et de revendications sur le serveur d'identité.  
Configuration de l'autorisation sur l'API pour la gestion des tâches et des équipes.
