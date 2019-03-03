﻿using System.Threading.Tasks;
using Bing.Biz.OAuthLogin.Core;
using Bing.Biz.OAuthLogin.Gitee.Configs;

namespace Bing.Biz.OAuthLogin.Gitee
{
    /// <summary>
    /// Gitee 授权提供程序
    /// </summary>
    public class GiteeAuthorizationProvider: AuthorizationProviderBase<IGiteeAuthorizationConfigProvider,GiteeAuthorizationConfig>,IGiteeAuthorizationProvider
    {
        /// <summary>
        /// PC端授权地址
        /// </summary>
        internal const string PcAuthorizationUrl = "https://gitee.com/oauth/authorize";

        /// <summary>
        /// PC端获取访问令牌地址
        /// </summary>
        internal const string PcAccessTokenUrl = "https://gitee.com/oauth/token";

        /// <summary>
        /// 跟踪日志名
        /// </summary>
        internal const string TraceLogName = "GiteeTraceLog";

        /// <summary>
        /// 初始化一个<see cref="GiteeAuthorizationProvider"/>类型的实例
        /// </summary>
        /// <param name="provider">Gitee 授权配置提供程序</param>
        public GiteeAuthorizationProvider(IGiteeAuthorizationConfigProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="builder">授权参数生成器</param>
        /// <param name="param">授权参数</param>
        /// <param name="config">授权配置</param>
        protected override void Config(AuthorizationParameterBuilder builder, AuthorizationParam param, GiteeAuthorizationConfig config)
        {
            builder.GatewayUrl(PcAuthorizationUrl)
                .ClientId(config.AppId)
                .RedirectUri(string.IsNullOrWhiteSpace(param.RedirectUri) ? config.CallbackUrl : param.RedirectUri)
                .ResponseType(param.ResponseType)
                .State(param.State);
        }

        /// <summary>
        /// 获取跟踪日志名
        /// </summary>
        /// <returns></returns>
        protected override string GetTraceLogName() => TraceLogName;

        /// <summary>
        /// 获取授权方式
        /// </summary>
        /// <returns></returns>
        protected override OAuthWay GetOAuthWay() => OAuthWay.Gitee;

        /// <summary>
        /// 生成授权地址
        /// </summary>
        /// <param name="request">Gitee 授权请求</param>
        /// <returns></returns>
        public async Task<string> GenerateUrlAsync(GiteeAuthorizationRequest request) => await GenerateUrlAsync(request.ToParam());
    }
}
