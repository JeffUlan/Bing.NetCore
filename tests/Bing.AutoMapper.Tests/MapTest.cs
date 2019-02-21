using System;
using System.Collections.Generic;
using Bing.AutoMapper.Tests.Samples;
using Bing.Mapping;
using Bing.Utils.Helpers;
using Xunit;

namespace Bing.AutoMapper.Tests
{
    /// <summary>
    /// AutoMapper ����ӳ�����
    /// </summary>
    public class MapTest
    {
        public MapTest()
        {
            var mapper = new AutoMapperMapper();
            MapperExtensions.SetMapper(mapper);
        }

        /// <summary>
        /// ����ӳ��
        /// </summary>
        [Fact]
        public void Test_MapTo()
        {
            Sample sample = new Sample();
            Sample2 sample2 = new Sample2() { StringValue = "a" };
            sample2.MapTo(sample);

            Assert.Equal("a", sample.StringValue);
        }

        /// <summary>
        /// ����ӳ��
        /// </summary>
        [Fact]
        public void Test_MapTo_2()
        {
            Sample sample = new Sample() { StringValue = "a" };
            Sample2 sample2 = sample.MapTo<Sample2>();

            Assert.Equal("a", sample2.StringValue);
        }

        /// <summary>
        /// ����ӳ�伯��
        /// </summary>
        [Fact]
        public void Test_MapTo_List()
        {
            List<Sample> sampleList = new List<Sample>() { new Sample() { StringValue = "a" }, new Sample() { StringValue = "b" } };
            List<Sample2> sample2List = new List<Sample2>();
            sampleList.MapTo(sample2List);

            Assert.Equal(2, sample2List.Count);
            Assert.Equal("a", sample2List[0].StringValue);
        }

        /// <summary>
        /// ����ӳ�伯��
        /// </summary>
        [Fact]
        public void Test_MapTo_List_2()
        {
            List<Sample> sampleList = new List<Sample>() { new Sample() { StringValue = "a" }, new Sample() { StringValue = "b" } };
            List<Sample2> sample2List = sampleList.MapTo<List<Sample2>>();

            Assert.Equal(2, sample2List.Count);
            Assert.Equal("a", sample2List[0].StringValue);
        }

        /// <summary>
        /// ����ӳ�伯��
        /// </summary>
        [Fact]
        public void Test_MapToList()
        {
            List<Sample> sampleList = new List<Sample>() { new Sample() { StringValue = "a" }, new Sample() { StringValue = "b" } };
            List<Sample2> sample2List = sampleList.MapToList<Sample2>();

            Assert.Equal(2, sample2List.Count);
            Assert.Equal("a", sample2List[0].StringValue);
        }

        /// <summary>
        /// ӳ�伯�� - ���Կռ���
        /// </summary>
        [Fact]
        public void Test_MapToList_Empty()
        {
            List<Sample> sampleList = new List<Sample>();
            List<Sample2> sample2List = sampleList.MapToList<Sample2>();

            Assert.Empty(sample2List);
        }

        /// <summary>
        /// ӳ�伯�� - ��������
        /// </summary>
        [Fact]
        public void Test_MapToList_Array()
        {
            Sample[] sampleList = new Sample[] { new Sample() { StringValue = "a" }, new Sample() { StringValue = "b" } };
            List<Sample2> sample2List = sampleList.MapToList<Sample2>();

            Assert.Equal(2, sample2List.Count);
            Assert.Equal("a", sample2List[0].StringValue);
        }

        /// <summary>
        /// ��������
        /// </summary>
        [Fact]
        public void Test_MapTo_MultipleThread()
        {
            Thread.ParallelExecute(() => {
                var sample = new Sample { StringValue = "a" };
                var sample2 = sample.MapTo<Sample2>();
                Assert.Equal("a", sample2.StringValue);
            }, 20);
        }

        /// <summary>
        /// ���Ժ�������
        /// </summary>
        [Fact]
        public void TestMapTo_Ignore()
        {
            DtoSample sample2 = new DtoSample { Name = "a", IgnoreValue = "b" };
            EntitySample sample = sample2.MapTo<EntitySample>();
            Assert.Equal("a", sample.Name);
            Assert.Null(sample.IgnoreValue);
            DtoSample sample3 = sample.MapTo<DtoSample>();
            Assert.Equal("a", sample3.Name);
            Assert.Null(sample3.IgnoreValue);
        }

        /// <summary>
        /// ����Castle������
        /// </summary>
        [Fact]
        public void TestMapTo_CastleProxy()
        {
            Castle.DynamicProxy.ProxyGenerator proxyGenerator = new Castle.DynamicProxy.ProxyGenerator();
            var proxy = proxyGenerator.CreateClassProxy<DtoSample>();
            proxy.Name = "a";
            EntitySample sample = proxy.MapTo<EntitySample>();
            Assert.Equal("a", sample.Name);

            var proxy2 = proxyGenerator.CreateClassProxy<DtoSample>();
            proxy2.Name = "b";
            sample = proxy2.MapTo<EntitySample>();
            Assert.Equal("b", sample.Name);
        }
    }
}
