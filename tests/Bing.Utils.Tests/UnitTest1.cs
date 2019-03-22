using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Bing.Utils.Extensions;
using Bing.Utils.Helpers;
using Bing.Utils.IdGenerators.Core;
using Bing.Utils.IO;
using Bing.Utils.Json;
using Xunit;
using Xunit.Abstractions;

namespace Bing.Utils.Tests
{
    public class UnitTest1: TestBase
    {
        public UnitTest1(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Test1()
        {
            DateTimeOffset offset=DateTimeOffset.Now;
            Output.WriteLine(offset.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [Fact]
        public void Test_GetArrayLowerBound()
        {
            var strs = new string[] {"1", "2", "3", "4", "5"};
            var lowerBound = strs.GetLowerBound(0);
            Output.WriteLine(lowerBound.ToString());
        }

        [Fact]
        public void Test_GetArrayLowerBound_2()
        {            
            string[,] strs = new string[,] {
                {
                    "1","2","3","4","7"
                },
                {
                    "2","3","4","5","6"
                }
            };
            var lowerBound = strs.GetLowerBound(1);
            Output.WriteLine(lowerBound.ToString());
            var upperBound = strs.GetUpperBound(1);
            Output.WriteLine(upperBound.ToString());
        }

        [Fact]
        public void Test_GetArrayUpperBound()
        {
            var strs = new string[] { "1", "2", "3", "4", "5" };
            var upperBound = strs.GetUpperBound(0);
            Output.WriteLine(upperBound.ToString());
        }

        [Fact]
        public void Test_Except()
        {
            var list=new string[]{"1","2","3","4","5"};
            var newList = new string[] {"1", "2"};
            var result=list.Except(newList);
            Output.WriteLine(result.ToJson());
        }

        [Fact]
        public void Test_Except_Int()
        {
            var list = new int[] {1, 3, 5, 7, 9, 11};
            var newList = new int[] {1, 4, 7,8, 9, 11};
            var result = newList.Except(list);
            Output.WriteLine(result.ToJson());
        }

        [Fact]
        public void Test_Except_Guid()
        {
            var oneGuid = Guid.NewGuid();
            var twoGuid = Guid.NewGuid();
            var threeGuid = Guid.NewGuid();
            var fourGuid = Guid.NewGuid();
            var fiveGuid = Guid.NewGuid();
            var sixGuid = Guid.NewGuid();
            var sevenGuid = Guid.NewGuid();
            var list = new Guid[] {oneGuid, twoGuid, threeGuid, fourGuid, fiveGuid, sixGuid, sevenGuid};
            var newList = new Guid[] { fourGuid, fiveGuid, sixGuid, sevenGuid };
            var result = list.Except(newList);
            Output.WriteLine(list.ToJson());
            Output.WriteLine(newList.ToJson());
            Output.WriteLine(result.ToJson());
        }

        [Fact]
        public void Test_GetDomainName()
        {
            string url1 = "https://www.cnblogs.com";
            string url2 = "https://www.cnblogs.com/";
            string url3 = "https://www.cnblogs.com/jeffwongishandsome/archive/2010/10/14/1851217.html";
            string url4 = "http://www.cnblogs.com/jeffwongishandsome/archive/2010/10/14/1851217.html";
            string url5 = "https://www.cnblogs.gz.org/";
            Output.WriteLine(GetDomainName(url1));
            Output.WriteLine(GetDomainName(url2));
            Output.WriteLine(GetDomainName(url3));
            Output.WriteLine(GetDomainName(url4));
            Output.WriteLine(GetDomainName(url5));
        }

        private string GetDomainName(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            Regex regex=new Regex(@"(http|https)://(?<domain>[^(:|/]*)", RegexOptions.IgnoreCase);
            return regex.Match(url, 0).Value;
        }

        [Fact]
        public void Test_Id()
        {
            for (int i = 0; i < 1000; i++)
            {
                var id = SequentialGuidGenerator.Current.Create();
                Output.WriteLine($"{id}");
            }
        }

        [Fact]
        public void Test_GetJson()
        {
            string jsonp = @"jsonp({""a"":""1234"",""b"":9999})";
            var json = "{\"a\":\"1234\",\"b\":9999}";
            var result = Regexs.GetValue(jsonp, @"^\w+\((\{[^()]+\})\)$","$1");
            Output.WriteLine(result);
            Assert.Equal(json,result);
        }

        [Fact]
        public void Test_SpaceOnUpper()
        {
            var words = new[] {"StringExtensions", "AA", "AbC", "Cad"};
            foreach (var word in words)
            {
                Output.WriteLine(word.SpaceOnUpper());
            }
        }

        [Fact]
        public void Test_RemoveStrat()
        {
            string path = "/Pages/Home/Index/Pages";
            var index = path.IndexOf(path, StringComparison.OrdinalIgnoreCase);
            var result = path.Remove(index, "/Paegs".Length).Insert(index, "/typings/app");
            Output.WriteLine(result);
        }

        /// <summary>
        /// ���ɲ�������
        /// </summary>
        [Fact]
        private void Test_GenerateTestData()
        {
            var dirPath = "D:\\TestData";
            DirectoryUtil.CreateIfNotExists(dirPath);
            FileSplit("D:\\iTestRunner_R1.txt", dirPath, 2048, false, -1);
        }

        /// <summary>
        /// �ϲ��ļ�
        /// </summary>
        [Fact]
        public void Test_CombineFile()
        {
            var soureMd5 = FileUtil.GetFileMd5("D:\\iTestRunner_R1.txt");
            var files = FileUtil.GetAllFiles("D:\\TestData");
            var outputFilePath = "D:\\iTestRunner_R1_combine_result.txt";
            FileCombine(files, outputFilePath, false, -1);
            var outputMd5 = FileUtil.GetFileMd5(outputFilePath);
            Output.WriteLine($"old-md5:{soureMd5}");
            Output.WriteLine($"new-md5:{outputMd5}");
        }

        /// <summary>
        /// �ļ��и�
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <param name="outPutPath">����ļ�·��</param>
        /// <param name="kbLength">�������ļ���󳤶ȡ���λ��KB</param>
        /// <param name="delete">��ʶ�ļ��ָ���ɺ��Ƿ�ɾ��ԭ�ļ�</param>
        /// <param name="change">������Կ</param>
        private void FileSplit(string filePath,string outPutPath, int kbLength, bool delete, int change)
        {
            if (filePath == null || !File.Exists(filePath))
            {
                return;
            }

            //// ���ܳ�ʼ��
            //short sign = 1;
            //int num = 0, tmp;
            //if (change < 0)
            //{
            //    sign = -1;
            //    change = -change;
            //}

            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var fileSize = FileUtil.GetFileSize(filePath);
            var total = Conv.ToInt(fileSize.GetSizeByK() / kbLength);
            using (FileStream readStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] data = new byte[1024];// ����ȡ������ռ�
                int len = 0, i = 1;// ��¼���ļ��ۻ���ȡ��KB��С���ָ�����ļ����
                int readLen = 0;// ÿ��ʵ�ʶ�ȡ���ֽڴ�С
                FileStream writeStream = null;
                // ��ȡ����
                while (readLen > 0 || (readLen = readStream.Read(data, 0, data.Length)) > 0)
                {                    
                    // �����ָ������ļ��������򸲸�
                    if (len == 0 || writeStream == null)
                    {
                        writeStream = new FileStream($"{outPutPath}\\{fileName}.{i++}.{total}.bin", FileMode.Create);
                    }

                    //// �����߼�����data�����ֽڽ����߼�ƫ�Ƽ���
                    //if (num == 0)
                    //{
                    //    num = change;
                    //}

                    //tmp = data[0] + sign * (num % 3 + 3);
                    //if (tmp > 255)
                    //{
                    //    tmp -= 255;
                    //}
                    //else if(tmp<0)
                    //{
                    //    tmp += 255;
                    //}

                    //data[0] = (byte) tmp;
                    //num /= 3;

                    // �������������д�����ļ�
                    writeStream.Write(data, 0, readLen);
                    writeStream.Flush();
                    // Ԥ����һ�ֻ�������
                    readLen = readStream.Read(data, 0, data.Length);
                    if (++len >= kbLength || readLen == 0) //���ļ��ﵽָ����С�����ļ��Ѷ���
                    {
                        writeStream.Close();// �رյ�ǰ�����
                        len = 0;
                    }
                }
            }

            if (delete)
            {
                FileUtil.Delete(filePath);
            }
        }

        /// <summary>
        /// �ļ��ϲ�
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="outFileName"></param>
        /// <param name="delete"></param>
        /// <param name="change"></param>
        private void FileCombine(IList<string> filePaths,string outFileName, bool delete, int change)
        {
            if (filePaths == null || filePaths.Count == 0)
            {
                return;
            }

            short sign = 1;
            //int num = 0, tmp;
            //if (change < 0)
            //{
            //    sign = -1;
            //    change = -change;
            //}

            var keys = Path.GetFileName(filePaths[0]).Split('.');
            var total = keys[2].ToInt();

            using (FileStream writeStream = new FileStream(outFileName, FileMode.Create))
            {
                filePaths.Sort();
                
                foreach (var filePath in filePaths)
                {
                    if (filePath == null || !File.Exists(filePath))
                    {
                        continue;
                    }

                    FileStream readStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    byte[] data=new byte[1024];// ����ȡ������ռ�
                    int readLen = 0;// ÿ��ʵ�ʶ�ȡ���ֽڴ�С

                    // ��ȡ����
                    while ((readLen=readStream.Read(data,0,data.Length))>0)
                    {
                        //// �����߼�����data�����ֽڽ����߼�ƫ�ƽ���
                        //if (num == 0)
                        //{
                        //    num = change;
                        //}

                        //tmp = data[0] + sign * (num % 3 + 3);
                        //if (tmp > 255)
                        //{
                        //    tmp -= 255;
                        //}
                        //else if(tmp<0)
                        //{
                        //    tmp += 255;
                        //}

                        //data[0] = (byte) tmp;
                        //num /= 3;

                        writeStream.Write(data, 0, readLen);
                        writeStream.Flush();
                    }

                    readStream.Close();

                    if (delete)
                    {
                        FileUtil.Delete(filePath);
                    }
                }
            }
        }
    }
}

