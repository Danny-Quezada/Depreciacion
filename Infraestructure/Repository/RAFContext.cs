﻿
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class RAFContext
    {
        private string fileName;
        private int size;
        private int NumberEliminated;

        public RAFContext(string fileName, int size)
        {
            this.fileName = fileName;
            this.size = size;
        }
        private Stream Eliminated
        {
            get => File.Open("Eliminated.hd", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
        
        public Stream HeaderStream
        {
            get => File.Open($"{fileName}.hd", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public Stream DataStream
        {
            get => File.Open($"{fileName}.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public void Create<T>(T t)
        {
            try
            {
                using (BinaryWriter bwHeader = new BinaryWriter(HeaderStream),
                                 bwData = new BinaryWriter(DataStream))
                {
                    int n, k;
                    using (BinaryReader brHeader = new BinaryReader(bwHeader.BaseStream))
                    {
                        if (brHeader.BaseStream.Length == 0)
                        {
                            n = 0;
                            k = 0;
                        }
                        else
                        {
                            brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                            n = brHeader.ReadInt32();
                            k = brHeader.ReadInt32();
                        }
                        //calculamos la posicion en Data
                        long pos = k * size;
                        bwData.BaseStream.Seek(pos, SeekOrigin.Begin);

                        PropertyInfo[] info = t.GetType().GetProperties();
                        foreach (PropertyInfo pinfo in info)
                        {
                            Type type = pinfo.PropertyType;
                            object obj = pinfo.GetValue(t, null);

                            if (type.IsGenericType)
                            {
                                continue;
                            }

                            if (pinfo.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                            {
                                bwData.Write(++k);
                                continue;
                            }

                            if (type == typeof(int))
                            {
                                bwData.Write((int)obj);
                            }
                            else if (type == typeof(long))
                            {
                                bwData.Write((long)obj);
                            }
                            else if (type == typeof(float))
                            {
                                bwData.Write((float)obj);
                            }
                            else if (type == typeof(double))
                            {
                                bwData.Write((double)obj);
                            }
                            else if (type == typeof(decimal))
                            {
                                bwData.Write((decimal)obj);
                            }
                            else if (type == typeof(char))
                            {
                                bwData.Write((char)obj);
                            }
                            else if (type == typeof(bool))
                            {
                                bwData.Write((bool)obj);
                            }
                            else if (type == typeof(string))
                            {
                                bwData.Write((string)obj);
                            }
                        }

                        long posh = 8 + n * 4;
                        bwHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                        bwHeader.Write(k);

                        bwHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                        bwHeader.Write(++n);
                        bwHeader.Write(k);
                    }
                    
                }
            }
            catch (IOException)
            {
                throw;
            }
        }

        public T Get<T>(int id)
        {
            try
             {
           
                T newValue = (T)Activator.CreateInstance(typeof(T));
                using (BinaryReader brHeader = new BinaryReader(HeaderStream),
                                    brData = new BinaryReader(DataStream))
                {
                    int n, k;
                    brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                    if (brHeader.BaseStream.Length == 0)
                    {
                        n = 0;
                        k = 0;
                        return newValue;
                    }
                    n = brHeader.ReadInt32();
                    k = brHeader.ReadInt32();
                    if (id <= 0 || id > k)
                    {
                        return default(T);
                    }
                    PropertyInfo[] properties = newValue.GetType().GetProperties();

             
                    long posh = 8 + (id - 1) * 4;
            
                    //TODO Add Binary search to find the id
                    brHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                    int index = brHeader.ReadInt32();
                    //TODO VALIDATE INDEX
                    long posd = (index - 1) * size;
                    brData.BaseStream.Seek(posd, SeekOrigin.Begin);
                    foreach (PropertyInfo pinfo in properties)
                    {
                        Type type = pinfo.PropertyType;

                        if (type.IsGenericType)
                        {
                            continue;
                        }

                        if (type == typeof(int))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<int>(TypeCode.Int32));
                        }
                        else if (type == typeof(long))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<long>(TypeCode.Int64));
                        }
                        else if (type == typeof(float))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<float>(TypeCode.Single));
                        }
                        else if (type == typeof(double))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<double>(TypeCode.Double));
                        }
                        else if (type == typeof(decimal))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<decimal>(TypeCode.Decimal));
                        }
                        else if (type == typeof(char))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<char>(TypeCode.Char));
                        }
                        else if (type == typeof(bool))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<bool>(TypeCode.Boolean));
                        }
                        else if (type == typeof(string))
                        {
                            pinfo.SetValue(newValue, brData.GetValue<string>(TypeCode.String));
                        }
                    }




                    return newValue;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Update<T>(T t)
        {
            int Id = int.Parse(t.GetType().GetProperty("Id").GetValue(t).ToString());

            using (BinaryReader brHeader = new BinaryReader(HeaderStream),
                                brData = new BinaryReader(DataStream))
            {
                int n, k;
                brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                if (brHeader.BaseStream.Length == 0)
                {
                    n = 0;
                    k = 0;
                    return;
                }
                n = brHeader.ReadInt32();
                k = brHeader.ReadInt32();

                long posh = 8 + (Id - 1) * 4;
                brHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                long index = brHeader.ReadInt32();
                long posd = (index - 1) * size;
                brData.Close();
                brHeader.Close();
                using (BinaryWriter binaryHeader = new BinaryWriter(HeaderStream),
                                   binaryData = new BinaryWriter(DataStream))
                {
                    PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                   
                  
                    
                    binaryData.BaseStream.Seek(posd, SeekOrigin.Begin);
                    foreach (PropertyInfo pinfo in propertyInfo)
                    {
                        Type type = pinfo.PropertyType;
                        object obj = pinfo.GetValue(t, null);

                        if (type.IsGenericType)
                        {
                            continue;
                        }
                        if (type == typeof(int))
                        {
                            binaryData.Write((int)obj);
                        }
                        else if (type == typeof(long))
                        {
                            binaryData.Write((long)obj);
                        }
                        else if (type == typeof(float))
                        {
                            binaryData.Write((float)obj);
                        }
                        else if (type == typeof(double))
                        {
                            binaryData.Write((double)obj);
                        }
                        else if (type == typeof(decimal))
                        {
                            binaryData.Write((decimal)obj);
                        }
                        else if (type == typeof(char))
                        {
                            binaryData.Write((char)obj);
                        }
                        else if (type == typeof(bool))
                        {
                            binaryData.Write((bool)obj);
                        }
                        else if (type == typeof(string))
                        {
                            binaryData.Write((string)obj);
                        }
                    }
                }
            }
        }
        public bool Delete(int id)
        {
            using(BinaryReader brHeader=new BinaryReader(HeaderStream))
            {
                int n, k;
                brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                if (brHeader.BaseStream.Length == 0)
                {
                    n = 0;
                    k = 0;
                    return false;
                }
                n = brHeader.ReadInt32();
                k = brHeader.ReadInt32();
                if (id <= 0 || id > k)
                {
                    return false;
                }
                long positionInitial = 8 + (id - 1) * 4;
                long positionFinal = 8 + (id * 4);
                using(BinaryWriter brHeaderTemp=new BinaryWriter(File.Open("Temp.txt",FileMode.Create,FileAccess.ReadWrite)))
                {
                    long length = brHeader.BaseStream.Length;
                    brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                    while (brHeader.BaseStream.Position < length)
                    {
                       
                        if (brHeader.BaseStream.Position == positionInitial)
                        {
                            brHeaderTemp.Write(-1);
                            brHeader.BaseStream.Position = brHeader.BaseStream.Position + 4;
                        }
                        else
                        {
                            brHeaderTemp.Write(brHeader.ReadInt32());
                        }
                       
                    }
                    brHeaderTemp.BaseStream.Seek(0, SeekOrigin.Begin);
                    brHeaderTemp.Write(n);
                    brHeaderTemp.Write(k);
                    
                }
            }
            using(BinaryWriter binaryWriter=new BinaryWriter(Eliminated))
            {
         
                binaryWriter.Write(++NumberEliminated);
                binaryWriter.Close();
            }
            File.Delete($"{fileName}.hd");
            File.Copy("Temp.txt", $"{fileName}.hd");
            return true;

        }
        public List<T> GetAll<T>()
        {
            List<T> listT = new List<T>();
            int n, k;
            using (BinaryReader brHeader = new BinaryReader(HeaderStream))
            {
                if (brHeader.BaseStream.Length == 0)
                {
                    return listT;
                }
                brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                n = brHeader.ReadInt32();
                k = brHeader.ReadInt32();
            }
            using (BinaryReader binaryReader=new BinaryReader(Eliminated))
            {
                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                if (binaryReader.BaseStream.Length == 0)
                {
                    NumberEliminated = 0;
                }
                else
                {
                    NumberEliminated = binaryReader.ReadInt32();
                }
                
            }
            for (int i = 0; i < n; i++)
            {
                int index;
                using (BinaryReader brHeader = new BinaryReader(HeaderStream))
                {

                    long posh = 8 + i * 4;
                    brHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                    index = brHeader.ReadInt32();
                    if (index == -1)
                    {
                        continue;
                    }
                }

                T t = Get<T>(index);
                listT.Add(t);
            }

            return listT;
        }

        public List<T> Find<T>(Expression<Func<T, bool>> where)
        {
            List<T> listT = new List<T>();
            int n, k;
            try
            {
                Func<T, bool> comparator = where.Compile();
                using (BinaryReader brHeader = new BinaryReader(HeaderStream))
                {
                    brHeader.BaseStream.Seek(0, SeekOrigin.Begin);
                    n = brHeader.ReadInt32();
                    k = brHeader.ReadInt32();
                }

                for (int i = 0; i < n; i++)
                {
                    int index;
                    using (BinaryReader brHeader = new BinaryReader(HeaderStream))
                    {
                        long posh = 8 + i * 4;
                        brHeader.BaseStream.Seek(posh, SeekOrigin.Begin);
                        index = brHeader.ReadInt32();
                    }

                    T t = Get<T>(index);
                    if (comparator(t))
                    {
                        listT.Add(t);
                    }

                }
                return listT;
            }
            catch (Exception)
            {
                throw;
            }
        }
           
    }
}
