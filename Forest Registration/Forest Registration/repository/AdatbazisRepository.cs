﻿using Forest_Register.modell;
using Forest_Register.repository;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forest_Registration.repository
{
    class AdatbazisRepository
    {
        private readonly string connectionString;
        private readonly string connectionStringTest;

        public AdatbazisRepository()
        {
            ConnectionString cs = new ConnectionString();
            connectionString = cs.getConnectionString();
            connectionStringTest = cs.getCreateString();
        }

        public void AdatbazisLetrehozas()
        {
            MySqlConnection connection = new MySqlConnection(connectionStringTest);
            try
            {
                string query = "CREATE DATABASE IF NOT EXISTS erdo_adatbazis DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci";
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Az adatbázis létrehozása nem sikerült vagy már létezik");
            }
        }

        public void AdatbazisTorles()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "DROP DATABASE `erdo_adatbazis`";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Az adatbázis törlése nem sikerült!");
            }
        }

        public void FelhasznalokTablaLetrehozas()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            string use = "USE erdo_adatbazis";
                string query = "CREATE TABLE IF NOT EXISTS `felhasznalok` (" +
                "`felhasznaloId` int(11) NOT NULL AUTO_INCREMENT, " +
                "`nev` varchar(20) COLLATE utf8_hungarian_ci NOT NULL, " +
                "`cim` varchar(50) COLLATE utf8_hungarian_ci NOT NULL, " +
                "`email` varchar(50) COLLATE utf8_hungarian_ci NOT NULL, " +
                "`jelszo` varchar(20) COLLATE utf8_hungarian_ci NOT NULL, " +
                "`rendszergazdaE` int(11) NOT NULL, " +
                "PRIMARY KEY(`felhasznaloId`), UNIQUE KEY `rendszergazdaE` (`felhasznaloId`)) " +
                "ENGINE = InnoDB AUTO_INCREMENT = 20 DEFAULT CHARSET = utf8 COLLATE = utf8_hungarian_ci;";
            try
            {
                connection.Open();
                MySqlCommand cmdUse = new MySqlCommand(use, connection);
                MySqlCommand cmdQuery = new MySqlCommand(query, connection);
                cmdUse.ExecuteNonQuery();
                cmdQuery.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen tábla létrehozás.");
            }
        }

        public void FelhasznalokTesztAdatokFeltoltese()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "INSERT IGNORE INTO `felhasznalok` (`felhasznaloId`, `nev`, `cim`, `email`, `jelszo`, `rendszergazdaE`) VALUES " +
                    "(5, 'Ferenczi Tamás', '', 'darius.517.ft@gmail.com', 'Jelszo1', 1), " +
                    "(6, 'Favágó Jani', '', 'favagojani@cim.hu', 'Jelszo2', 0), " +
                    "(9, 'Bükki Jenő', '', 'bukki.jeno@citromail.hu', 'jelszo3', 0), " +
                    "(10, 'Erik', '', 'e@g.com', 'Jelszo8', 0), " +
                    "(11, 'Márk', '', 'm@g.com', 'Jelszo9', 0), " +
                    "(12, 'Kecske Tej', '', 'k@g.com', 'Jelszo10', 0), " +
                    "(13, 'Fa Gergo', '', 'f@g.com', 'Jelszo11', 0), " +
                    "(14, 'Teszt Elek', '', 't@g.com', 'Jelszo12', 0), " +
                    "(15, 'Erdősítő Zoltán', '', 'ez@g.com', 'Jelszo13', 0), " +
                    "(16, 'Erik', '', 'r@g.com', 'Jelszo14', 0), " +
                    "(17, 'Ákos', '', 'a@g.com', 'Jelszo15', 0), " +
                    "(18, 'Tomi', '', 't@g.com', 'Jelszo16', 0), " +
                    "(19, 'Zoli', '', 'z@g.com', 'Jelszo17', 0); ";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen teszt adat feltöltés.");
            }
        }

        /*public void FelhasznalokTablaTorles()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                string use = "USE erdo_adatbazis";
                string query = "DROP TABLE felhasznalok";
                MySqlCommand cmdUse = new MySqlCommand(use, connection);
                MySqlCommand cmdQuery = new MySqlCommand(query, connection);
                cmdUse.ExecuteNonQuery();
                cmdQuery.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen tábla törlés.");
            }
        }

        public void FelhasznalokAdatTorlesTablabol()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "DELETE FROM `felhasznalok`";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen adat törlés");
            }
        }*/

        public void FafajokTablaLetrehozas()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string use = "USE erdo_adatbazis";
                string query = "CREATE TABLE IF NOT EXISTS `fafajok` ( " +
                    "`fafajId` int(11) NOT NULL AUTO_INCREMENT, " +
                    "`megnevezes` varchar(20) COLLATE utf8_hungarian_ci NOT NULL, " +
                    "PRIMARY KEY(`fafajId`))" +
                    " ENGINE = InnoDB AUTO_INCREMENT = 11 DEFAULT CHARSET = utf8 COLLATE = utf8_hungarian_ci; ";
                MySqlCommand cmdUse = new MySqlCommand(use, connection);
                MySqlCommand cmdQuery = new MySqlCommand(query, connection);
                cmdUse.ExecuteNonQuery();
                cmdQuery.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen tábla létrehozás.");
            }
        }

        /*public void FafajokTablaTorlese()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string use = "USE erdo_adatbazis";
                string query = "DROP TABLE fafajok";
                MySqlCommand cmdUse = new MySqlCommand(use, connection);
                MySqlCommand cmdQuery = new MySqlCommand(query, connection);
                cmdUse.ExecuteNonQuery();
                cmdQuery.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen tábla törlés.");
            }
        }*/

        public void FafajokTesztAdatokFeltoltese()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "INSERT IGNORE INTO `erdo_adatbazis`.`fafajok` (`fafajId`, `megnevezes`) VALUES " +
                    "(1, 'Bükk'), " +
                    "(2, 'Fűz fa'), " +
                    "(3, 'Galagonya'), " +
                    "(4, 'Gyertyán'), " +
                    "(5, 'Hárs'), " +
                    "(6, 'Jegenye fenyő'), " +
                    "(7, 'Juhar'), " +
                    "(8, 'Kecskerágó'), " +
                    "(9, 'Közönséges boroka'), " +
                    "(10, 'Éger');";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen teszt adat feltöltés.");
            }
        }

        /*public void FafajokAdatokTorlese()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "DELETE FROM `fafajok`";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen adat törlés");
            }
        }*/

        public void FakTablaLetrehozasa()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string use = "USE erdo_adatbazis";
                string query = "CREATE TABLE IF NOT EXISTS `fak` (" +
                    "`fafajId` int(11) NOT NULL, " +
                    "`mennyiseg` int(11) NOT NULL, " +
                    "`erdeszeti_azonosito` varchar(20) COLLATE utf8_hungarian_ci NOT NULL, " +
                    "KEY `fafajId` (`fafajId`), " +
                    "KEY `erdeszeti_azonosito` (`erdeszeti_azonosito`))" +
                    " ENGINE = InnoDB DEFAULT CHARSET = utf8 COLLATE = utf8_hungarian_ci; ";
                MySqlCommand cmdUse = new MySqlCommand(use, connection);
                MySqlCommand cmdQuery = new MySqlCommand(query, connection);
                cmdUse.ExecuteNonQuery();
                cmdQuery.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen tábla létrehozás.");
            }
        }

        public void FakIdegenKulcsok()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string queryKeys = "ALTER TABLE `fak` " +
                    "ADD CONSTRAINT `fak_ibfk_1` FOREIGN KEY IF NOT EXISTS (`fafajId`) REFERENCES `fafajok` (`fafajId`)  ON DELETE CASCADE ON UPDATE CASCADE, " +
                    "ADD CONSTRAINT `fak_ibfk_2` FOREIGN KEY IF NOT EXISTS (`erdeszeti_azonosito`) REFERENCES `erdok` (`erdeszeti_azonosito`)  ON DELETE CASCADE ON UPDATE CASCADE; ";
                MySqlCommand cmdKeys = new MySqlCommand(queryKeys, connection);
                cmdKeys.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen idegen kulcsok létrehozása.");
            }
        }

        public void FakTesztAdatokFeltoltese()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "INSERT IGNORE INTO `erdo_adatbazis`.`fak` (`fafajId`, `mennyiseg`, `erdeszeti_azonosito`) VALUES " +
                    " (3, 400, 'DASISTERDO422')," +
                    " (2, 20, 'ERDO555/')," +
                    " (4, 200, 'ERDO9'), " +
                    " (9, 111, 'valamiErdo');";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen teszt adat feltöltés.");
            }
        }

        /*public void FakTablaTorlese()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string use = "USE erdo_adatbazis";
                string query = "DROP TABLE fak";
                MySqlCommand cmdUse = new MySqlCommand(use, connection);
                MySqlCommand cmdQuery = new MySqlCommand(query, connection);
                cmdUse.ExecuteNonQuery();
                cmdQuery.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen tábla törlés.");
            }
        }

        public void FakAdatokTorlese()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "DELETE FROM `fak`";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen adat törlés");
            }
        }*/

        public void FahaszModTablaLetrehozas()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string use = "USE erdo_adatbazis";
                string query = "CREATE TABLE IF NOT EXISTS `fa_hasznalat_modjai` (" +
                    "`hasznalatId` int(11) NOT NULL AUTO_INCREMENT, " +
                    "`megnevezes` varchar(50) COLLATE utf8_hungarian_ci NOT NULL, " +
                    "`rovidites` varchar(10) COLLATE utf8_hungarian_ci NOT NULL, PRIMARY KEY(`hasznalatId`))" +
                    " ENGINE = InnoDB AUTO_INCREMENT = 5 DEFAULT CHARSET = utf8 COLLATE = utf8_hungarian_ci; ";
                MySqlCommand cmdUse = new MySqlCommand(use, connection);
                MySqlCommand cmdQuery = new MySqlCommand(query, connection);
                cmdUse.ExecuteNonQuery();
                cmdQuery.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen tábla létrehozás.");
            }
        }

        public void FaHaszModTesztAdatokFeltoltese()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "INSERT IGNORE INTO `erdo_adatbazis`.`fa_hasznalat_modjai` (`hasznalatId`, `megnevezes`, `rovidites`) VALUES " +
                    "(1, 'Tarvágás', 'TRV'), " +
                    "(2, 'Tisztítás', 'Ti'), " +
                    "(3, 'Egészségügyi Termelés', 'EÜ'), " +
                    "(4, 'Törzskiválaszó gyérítés', 'TKGY'); ";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen teszt adat feltöltés.");
            }
        }

        /*public void FaHaszModTablaTorles()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string use = "USE erdo_adatbazis";
                string query = "DROP TABLE fa_hasznalat_modjai";
                MySqlCommand cmdUse = new MySqlCommand(use, connection);
                MySqlCommand cmdQuery = new MySqlCommand(query, connection);
                cmdUse.ExecuteNonQuery();
                cmdQuery.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen tábla törlés.");
            }
        }

        public void FaHaszModAdatTorles()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "DELETE FROM `fa_hasznalat_modjai`";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Debug.WriteLine(e.Message);
                throw new RepositoryException("Sikertelen adat törlés");
            }
        }*/

        
    }
}
