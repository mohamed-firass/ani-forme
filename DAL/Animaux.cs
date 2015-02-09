﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using BO;
using DAL.Dapper;

namespace DAL
{
    public class Animaux
    {
        /// <summary>
        /// Récupére tout les animaux
        /// </summary>
        /// <returns></returns>
        public static List<BO.Animaux> GetAll()
        {
            try
            {
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                String query = @"SELECT * FROM Animaux a
                                 LEFT JOIN Clients c ON a.CodeClient = c.CodeClient
                                 ORDER BY a.CodeAnimal";

                List<BO.Animaux> results = cnx.Query<BO.Animaux, BO.Clients, BO.Animaux>(query,
                                           (animaux, client) => { animaux.Client = client; return animaux; },
                                           splitOn: "CodeClient")
                                           .ToList<BO.Animaux>();

                SqlConnexion.CloseConnexion(cnx);
                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Récupere tout les animaux archivé ou non
        /// </summary>
        /// <param name="archived"></param>
        /// <returns></returns>
        public static List<BO.Animaux> GetAll(bool archived)
        {
            try
            {
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                String query = @"SELECT * FROM Animaux a
                                 LEFT JOIN Clients c ON a.CodeClient = c.CodeClient
                                 WHERE a.Archive = @archive
                                 ORDER BY a.CodeAnimal";

                List<BO.Animaux> results = cnx.Query<BO.Animaux, BO.Clients, BO.Animaux>(query,
                                           (animaux, client) => { animaux.Client = client; return animaux; },
                                           param: new { archive = archived },
                                           splitOn: "CodeClient")
                                           .ToList<BO.Animaux>();

                SqlConnexion.CloseConnexion(cnx);
                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Récupére tout les animaux par clients, archivé ou non
        /// </summary>
        /// <param name="?"></param>
        /// <param name="archived"></param>
        /// <returns></returns>
        public static List<BO.Animaux> GetAllByClient(BO.Clients cli, bool archived)
        {
            try
            {
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                String query = @"SELECT * FROM Animaux a
                                 LEFT JOIN Clients c ON a.CodeClient = c.CodeClient
                                 WHERE a.CodeClient = @codeClient  
                                 AND a.Archive = @archive
                                 ORDER BY a.CodeAnimal";

                List<BO.Animaux> results = cnx.Query<BO.Animaux, BO.Clients, BO.Animaux>(query,
                                           (animaux, client) => { animaux.Client = client; return animaux; }, 
                                           param: new { archive = archived, codeClient = cli.CodeClient },
                                           splitOn: "CodeClient")
                                           .ToList<BO.Animaux>();

                SqlConnexion.CloseConnexion(cnx);
                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Récupére tout les animaux par clients
        /// </summary>
        /// <param name="cli"></param>
        /// <returns></returns>
        public static List<BO.Animaux> GetAllByClient(BO.Clients cli)
        {
            try
            {
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                String query = @"SELECT * FROM Animaux a
                                 LEFT JOIN Clients c ON a.CodeClient = c.CodeClient
                                 WHERE a.CodeClient = @codeClient  
                                 ORDER BY a.CodeAnimal";

                List<BO.Animaux> results = cnx.Query<BO.Animaux, BO.Clients, BO.Animaux>(query,
                                           (animaux, client) => { animaux.Client = client; return animaux; },
                                           param: new { codeClient = cli.CodeClient },
                                           splitOn: "CodeClient")
                                           .ToList<BO.Animaux>();

                SqlConnexion.CloseConnexion(cnx);
                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Retourne tout les animaux avec le nom comme %value%
        /// </summary>
        /// <param name="value"></param>
        /// <param name="archived"></param>
        /// <returns></returns>
        public static List<BO.Animaux> GetAllByNomAnimalArchive(String value, bool archived)
        {
            try
            {
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                String query = @"SELECT * FROM Animaux a
                                 LEFT JOIN Clients c ON a.CodeClient = c.CodeClient
                                 WHERE a.Archive = @archive
                                 AND a.NomAnimal LIKE (@value)
                                 ORDER BY a.CodeAnimal";

                List<BO.Animaux> results = cnx.Query<BO.Animaux, BO.Clients, BO.Animaux>(query,
                                           (animaux, client) => { animaux.Client = client; return animaux; },
                                           param: new { value = '%' + value + '%', archive = archived },
                                           splitOn: "CodeClient")
                                           .ToList<BO.Animaux>();

                SqlConnexion.CloseConnexion(cnx);
                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Retourne tout les animaux avec le client ayant comme nom %value%
        /// </summary>
        /// <param name="value"></param>
        /// <param name="archived"></param>
        /// <returns></returns>
        public static List<BO.Animaux> GetAllByNomClientArchive(String value, bool archived)
        {
            try
            {
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                String query = @"SELECT * FROM Animaux a
                                 LEFT JOIN Clients c ON a.CodeClient = c.CodeClient
                                 WHERE a.Archive = @archive
                                 AND c.NomClient LIKE (@value)
                                 ORDER BY a.CodeAnimal";

                List<BO.Animaux> results = cnx.Query<BO.Animaux, BO.Clients, BO.Animaux>(query,
                                           (animaux, client) => { animaux.Client = client; return animaux; },
                                           param: new { value = '%' + value + '%', archive = archived },
                                           splitOn: "CodeClient")
                                           .ToList<BO.Animaux>();

                SqlConnexion.CloseConnexion(cnx);
                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Retourne tout les animaux avec le tatouage comme value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="archived"></param>
        /// <returns></returns>
        public static List<BO.Animaux> GetAllByTatooArchive(String value, bool archived)
        {
            try
            {
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                String query = @"SELECT * FROM Animaux a
                                 LEFT JOIN Clients c ON a.CodeClient = c.CodeClient
                                 WHERE a.Archive = @archive
                                 AND a.Tatouage LIKE (@value)
                                 ORDER BY a.CodeAnimal";

                List<BO.Animaux> results = cnx.Query<BO.Animaux, BO.Clients, BO.Animaux>(query,
                                           (animaux, client) => { animaux.Client = client; return animaux; },
                                           param: new { value = '%' + value + '%', archive = archived },
                                           splitOn: "CodeClient")
                                           .ToList<BO.Animaux>();

                SqlConnexion.CloseConnexion(cnx);
                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Archive l'animal séléctionner
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        public static bool Archive(BO.Animaux animal)
        {
            return false;
        }

        /// <summary>
        /// Archive tout les animaux du client
        /// </summary>
        /// <param name="cli"></param>
        /// <returns></returns>
        public static bool ArchiveAllByClient(BO.Clients cli)
        {

            return false;
        }

        /// <summary>
        /// Creer l'animal passé en params
        /// Et le retourne avec son GUID
        /// </summary>
        /// <param name="animal"></param>
        public static BO.Animaux Create(BO.Animaux animal)
        {
            return null;
        }

        /// <summary>
        /// Met a jour l'animal passé en params
        /// </summary>
        /// <param name="animal"></param>
        public static bool Update(BO.Animaux animal)
        {
            return false;
        }
    }
}
