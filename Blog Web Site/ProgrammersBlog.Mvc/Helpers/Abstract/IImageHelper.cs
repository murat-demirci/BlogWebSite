﻿using Microsoft.AspNetCore.Http;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        string Upload(string name, IFormFile pictureFile,PictureType pictureType, string folderName = null);
        Task<IDataResult<ImageUploadedDto>> Uploaded(string name, IFormFile pictureFile, PictureType pictureType, string folderName = null);
        IDataResult<ImageDeletedDto> Delete(string pictureName);
    }
}