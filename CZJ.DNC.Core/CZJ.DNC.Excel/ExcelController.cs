﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZJ.DNC.Excel;
using Microsoft.AspNetCore.Mvc;
using CZJ.Common;
using CZJ.DNC.Net;
using CZJ.Common.Serializer;
using CZJ.Common.Core;

namespace CZJ.Excel.Controllers
{
    /// <summary>
    /// Excel导入导出接口
    /// </summary>
    [Route("api/[controller]")]
    public sealed class ExcelController : Controller
    {
        private readonly IEnumerable<ExcelImport> allImports;
        private readonly IObjectSerializer serializer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allImports"></param>
        /// <param name="serializer"></param>
        public ExcelController(IEnumerable<ExcelImport> allImports, IObjectSerializer serializer)
        {
            this.allImports = allImports;
            this.serializer = serializer;
        }

        /// <summary>
        /// 通用导出EXECL接口
        /// </summary>
        /// <param name="info">列表参数信息</param>
        /// <returns>内存流信息</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Export([FromBody]ExcelInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            string fileExt = info.GetFileExt();
            if (string.IsNullOrEmpty(info.FileName))
            {
                info.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
            }
            else
            {
                if (!info.FileName.EndsWith(fileExt))
                {
                    info.FileName = info.FileName + fileExt;
                }
            }
            if (!info.IsExportSelectData)
            {
                //设置最大导出条数
                info.Filter = info.Filter ?? new Dictionary<string, object>();
                if (string.IsNullOrEmpty(info.Api))
                {
                    throw new ArgumentNullException(nameof(info.Api));
                }
                if (!info.Api.StartsWith(Request.Scheme))
                {
                    info.Api = $"{Request.Scheme}://{Request.Host}{info.Api}";
                }
            }
            using (var ms = await info.ExportExeclStream(Request.Headers))
            {
                byte[] msbyte = ms.GetBuffer();
                Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                Response.Headers.Add("Content-Disposition", string.Format("attachment;filename={0}", Uri.EscapeUriString(info.FileName)));
                return File(msbyte, MimeHelper.GetMineType(info.FileName));
            }                     
        }

        /// <summary>
        /// 导出Excel模版
        /// </summary>
        /// <param name="type">业务类型</param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> DownLoadTemplate(string type)
        {
            var handler = GetExcelHandler(type);
            string extraParam = Request.Query["extra"];
            if (!string.IsNullOrWhiteSpace(extraParam))
            {
                handler.ExtraParam = extraParam;
            }
            string path = handler.TemplatePath;
            if (System.IO.File.Exists(path))
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    await handler.GetExportTemplate(ms);
                    ms.Position = 0;
                    Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                    Response.Headers.Add("Content-Disposition", string.Format("attachment;filename={0}", Uri.EscapeUriString(Path.GetFileName(path))));
                    return File(ms, MimeHelper.GetMineType(path));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("未找到“" + type.ToString() + "”对应模版文件");
            }
        }

        /// <summary>
        /// 导入Excel模版
        /// </summary>
        /// <param name="fileDoc">模板文件</param>
        /// <param name="type">业务类型</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ImportTemplate(string type, SwaggerFile fileDoc = null)
        {
            ImportResult result = new ImportResult();
            try
            {
                var handler = GetExcelHandler(type);
                string extraParam = Request.Query["extra"];
                if (!string.IsNullOrWhiteSpace(extraParam))
                {
                    handler.ExtraParam = extraParam;
                }
                //文件
                var file = Request.Form.Files[0];
                using (MemoryStream ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    ms.Position = 0;
                    result = await handler.ImportTemplate(ms, file.FileName);
                }
                if (result.IsSuccess)
                {
                    //是否获取详细数据，决定后台是否返回 result.ExtraInfo
                    string ReturnDetailData = Request.Query["ReturnDetailData"];
                    if (string.IsNullOrEmpty(ReturnDetailData) || ReturnDetailData != "1")
                    {
                        result.ExtraInfo = null;
                    }
                }
                else
                {
                    //设置错误模版http路径
                    result.Message = $"{Request.Scheme}://{Request.Host}{result.Message}";
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return Content(serializer.Serialize(result));
        }

        /// <summary>
        /// 获取相应类型对应的处理器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ExcelImport GetExcelHandler(string type)
        {
            if (allImports == null)
            {
                throw new ArgumentNullException("系统不存在Excel批量导入业务处理模块");
            }
            if (string.IsNullOrEmpty(type))
            {
                throw new Exception("请传入相应处理模块type");
            }
            var handler = allImports.FirstOrDefault(e => e.Type == type);
            if (handler == null)
            {
                throw new Exception("未找到“" + type.ToString() + "”相应处理模块");
            }
            return handler;
        }
    }
}