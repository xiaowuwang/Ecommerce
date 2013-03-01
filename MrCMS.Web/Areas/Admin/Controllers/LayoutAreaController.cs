﻿using System.Web.Mvc;
using MrCMS.Entities.Documents.Layout;
using MrCMS.Entities.Documents.Web;
using MrCMS.Helpers;
using MrCMS.Models;
using MrCMS.Services;
using MrCMS.Website.Binders;
using MrCMS.Website.Controllers;
using NHibernate;

namespace MrCMS.Web.Areas.Admin.Controllers
{
    public class LayoutAreaController : MrCMSAdminController
    {
        private readonly IDocumentService _documentService;
        private readonly ILayoutAreaService _layoutAreaService;

        public LayoutAreaController(ILayoutAreaService layoutAreaService, IDocumentService documentService)
        {
            _layoutAreaService = layoutAreaService;
            _documentService = documentService;
        }

        [HttpGet]
        public PartialViewResult Add(Layout layout)
        {
            return PartialView("Add", new LayoutArea { Layout = layout });
        }

        [HttpPost]
        public ActionResult Add(LayoutArea layoutArea)
        {
            _layoutAreaService.SaveArea(layoutArea);

            return RedirectToAction("Edit", "Layout", new { id = layoutArea.Layout.Id });
        }

        [HttpGet]
        [ActionName("Edit")]
        public ActionResult Edit_Get(LayoutArea layoutArea)
        {
            if (layoutArea == null)
                return RedirectToAction("Index", "Layout");

            return View(layoutArea);
        }

        [HttpPost]
        public ActionResult Edit(LayoutArea area)
        {
            _layoutAreaService.SaveArea(area);

            return RedirectToAction("Edit", "Layout", new { id = area.Layout.Id });
        }


        [HttpGet]
        [ActionName("Delete")]
        public ActionResult Delete_Get(LayoutArea area)
        {
            return PartialView(area);
        }

        [HttpPost]
        public ActionResult Delete(LayoutArea area)
        {
            var layoutId = area.Layout.Id;
            _layoutAreaService.DeleteArea(area);

            return RedirectToAction("Edit", "Layout", new { id = layoutId });
        }

        [HttpGet]
        public ActionResult SortWidgets(LayoutArea area)
        {
            return View(area.GetWidgets());
        }

        public void SortWidgetsAction(string orders)
        {
            _layoutAreaService.SetWidgetOrders(orders);
        }

        [HttpGet]
        public ActionResult SortWidgetsForPage(LayoutArea area, int pageId)
        {
            var webpage = _documentService.GetDocument<Webpage>(pageId);

            return View(new PageWidgetSortModel(area.GetWidgets(webpage), webpage, area));
        }

        [HttpPost]
        public ActionResult SortWidgetsForPage(PageWidgetSortModel pageWidgetSortModel)
        {
            _layoutAreaService.SetWidgetForPageOrders(pageWidgetSortModel);
            return RedirectToAction("Edit", "Webpage", new { id = pageWidgetSortModel.WebpageId });
        }

        [HttpPost]
        public ActionResult ResetSorting(LayoutArea area, int pageId)
        {
            var webpage = _documentService.GetDocument<Webpage>(pageId);
            _layoutAreaService.ResetSorting(area, webpage);

            return RedirectToAction("Edit", "Webpage", new { id = pageId });
        }
    }
}