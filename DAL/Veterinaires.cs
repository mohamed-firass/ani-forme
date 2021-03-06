﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DAL.Dapper;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class Veterinaires
    {
        /// <summary>
        /// Récupère tout les vétérinaires
        /// </summary>
        /// <returns></returns>
        public static List<BO.Veterinaires> GetAll()
        {
            try
            {
                var query = @"SELECT * FROM  Veterinaires v left join Account a on a.id = v.AccountId Order By v.CodeVeto";
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                List<BO.Veterinaires> results = cnx.Query<BO.Veterinaires, BO.Account, BO.Veterinaires>(query, (veto, account) => { veto.Account = account; return veto; }).ToList<BO.Veterinaires>();
                SqlConnexion.CloseConnexion(cnx);

                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Surcharge de la méthode getAll pour obtenir les archivés ou non
        /// </summary>
        /// <param name="archived"></param>
        /// <returns></returns>
        public static List<BO.Veterinaires> GetAll(bool archived)
        {
            try
            {
                var query = @"SELECT * FROM  Veterinaires v left join Account a on a.id = v.AccountId  WHERE v.Archive=@archive Order By v.CodeVeto";
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                List<BO.Veterinaires> results = cnx.Query<BO.Veterinaires, BO.Account, BO.Veterinaires>(query, (veto, account) => { veto.Account = account; return veto; }, new { archive = archived }).ToList<BO.Veterinaires>();
                SqlConnexion.CloseConnexion(cnx);

                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Recupere le veterinaire dont l'id est passé en parametre
        /// </summary>
        /// <param name="idParam"></param>
        /// <returns></returns>
        public static BO.Veterinaires Get(Guid idParam)
        {
            try
            {
                var query = @"SELECT * 
                                FROM  Veterinaires v 
                                LEFT JOIN Account a ON a.id = v.AccountId 
                                WHERE CodeVeto = @codeVeto";
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                List<BO.Veterinaires> results = cnx.Query<BO.Veterinaires, BO.Account, BO.Veterinaires>(query, (veto, account) => { veto.Account = account; return veto; }, new { codeVeto = idParam }).ToList<BO.Veterinaires>();
                SqlConnexion.CloseConnexion(cnx);

                return results.First();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Recupere le veterinaire dont l'idAccount est passé en parametre
        /// </summary>
        /// <param name="idParam"></param>
        /// <returns></returns>
        public static BO.Veterinaires GetByAccount(int idParam)
        {
            try
            {
                var query = @"SELECT * 
                                FROM  Veterinaires v 
                                LEFT JOIN Account a ON a.id = v.AccountId 
                                WHERE AccountId = @codeAccount";
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                List<BO.Veterinaires> results = cnx.Query<BO.Veterinaires, BO.Account, BO.Veterinaires>(query, 
                                                                                                        (veto, account) => { veto.Account = account; return veto; }, 
                                                                                                        new { codeAccount = idParam }
                                                                                                        ).ToList<BO.Veterinaires>();
                SqlConnexion.CloseConnexion(cnx);

                return results.First();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Déactive un vétérinaire
        /// </summary>
        /// <param name="veto"></param>
        /// <returns></returns>
        public static bool Archive(BO.Veterinaires vetoParams, bool archived)
        {
            try
            {
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                var query = @"UPDATE Veterinaires SET Archive=@archive WHERE CodeVeto = @codeVeto";
                int rowNb = cnx.Execute(query, new { codeVeto = vetoParams.CodeVeto, archive = (archived) ? 1 : 0 });
                SqlConnexion.CloseConnexion(cnx);
                return (rowNb > 0);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Creer un nouveau vétérinaire et le retourne avec son identifiant
        /// </summary>
        /// <param name="veterinaires"></param>
        /// <returns></returns>
        public static BO.Veterinaires Create(BO.Veterinaires vetoParams)
        {
            try
            {
                SqlConnection cnx = DAL.SqlConnexion.OpenConnexion();
                Guid temp = cnx.ExecuteScalar<Guid>(  "EXEC ajout_veterinaire @nomveto, @archive, @account",
                                        new { 
	                                            nomveto = vetoParams.NomVeto,
	                                            archive = (vetoParams.Archive) ? 1 : 0,
	                                            account = vetoParams.AccountId
                                        });
                vetoParams.CodeVeto = temp;
                SqlConnexion.CloseConnexion(cnx);

                return vetoParams;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
