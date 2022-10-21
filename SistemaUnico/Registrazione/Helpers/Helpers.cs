using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RegistrazioneSistemaUnico.Helpers.Attributes;
using RegistrazioneSistemaUnico.Helpers.Grid;
using RegistrazioneSistemaUnico.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RegistrazioneSistemaUnico.Helpers
{


	public class Column
	{
		public PropertyInfo Property { get; set; }
		public string Name { get; set; }
		public string HeaderName { get; set; }
		public string OrderBy { get; set; }
		public Type Type { get; set; }
		public object Item { get; set; }
		public string Size { get; set; }
		public Func<object, string> CustomResult { get; set; }
		public bool Orderable { get; set; } = true;
		public bool Visible { get; set; } = true;
	}




	public static class Helper
	{
		/// <summary>
		/// Renderizza un input dato un oggetto
		/// </summary>
		/// <typeparam name="TModel">Tipo dell'oggetto dal quale renderizzare una proprietà</typeparam>
		/// <typeparam name="TResult">Tipologia del risultato</typeparam>
		/// <param name="htmlHelper">oggetto derivato del contesto html</param>
		/// <param name="model">oggetto per il quale si vuole renderizzare una proprietà</param>
		/// <param name="propertyLambda">Proprietà dell'oggetto da renderizzare</param>
		/// <param name="cssClass">eventuali classi da aggiungere all'input</param>
		/// <param name="showRequiredOnLabel">Visualizzazione dell'asterisco come campo obbligatorio. Default=true</param>
		/// <param name="disabled">Indica se renderizzare l'input disabilitato o meno. Default=false</param>
		/// <param name="readOnly">Indica se renderizzare l'input di sola lettura o meno. Default=false</param>
		/// <returns></returns>
		public static IHtmlContent InputFor<TModel, TResult>(
			this IHtmlHelper htmlHelper,
			TModel model,
			Expression<Func<TModel, TResult>> propertyLambda,
			string cssClass = null,
			bool showRequiredOnLabel = true,
			bool disabled = false,
			bool readOnly = false,
			string label = null,
			string tooltip = null
			)
		{
			Type type = typeof(TModel);
			// Recupero proietà richiesta
			MemberExpression member = propertyLambda.Body as MemberExpression;
			if (member == null)
				throw new ArgumentException($"L'espressione '{propertyLambda}' si riferisce ad un metodo e non ad una proprietà.");


			PropertyInfo property = member.Member as PropertyInfo;
			if (property == null)
				throw new ArgumentException($"L'espressione '{propertyLambda}' si riferisce ad un campo e non ad una proprietà.");

			if (type != property.ReflectedType &&
				!type.IsSubclassOf(property.ReflectedType))
				throw new ArgumentException($"L'espressione '{propertyLambda}' si riferisca ad una proprietà che non è del tipo {type}.");

			if (cssClass != null && cssClass.Contains('\"'))
			{
				throw new ArgumentException($"La classe CSS non deve contenere caratteri \"");
			}
			DateAttribute dateAttribute = property.GetCustomAttribute<DateAttribute>();
			//Recupero Dati
			string value = "";
			object rawValue = null;
			try
			{
				if (model != null)
				{

					PropertyInfo propInfo = model.GetType().GetProperty(property.Name);
					if (propInfo != null)
					{
						rawValue = propInfo.GetValue(model);
						if (dateAttribute != null && (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?)))
						{
							value = ((DateTime?)propInfo.GetValue(model))?.ToString("dd/MM/yyyy");
						}
						else
						{
							value = propInfo.GetValue(model)?.ToString();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			string messageTextClass = "text-muted";
			string labelTextClass = "";
			string message = "";
			string hint = "";
			string inputCssClass = "form-control";

			HintAttribute hintAttribute = property.GetCustomAttribute<HintAttribute>();
			if (hintAttribute != null)
			{
				message = hintAttribute.Text;
				hint = $" data-val-hint=\"{message}\"";
			}
			PlaceholderAttribute placeholderAttribute = property.GetCustomAttribute<PlaceholderAttribute>();
			string attributes = "";
			if (disabled)
			{
				attributes += " disabled";
			}
			if (readOnly)
			{
				attributes += " readonly";
			}
			if (placeholderAttribute != null)
			{
				attributes += $" placeholder=\"{placeholderAttribute.Text}\"";
			}
			//Gestione Errori
			var error = htmlHelper.ViewData?.ModelState[property.Name]?.Errors.FirstOrDefault();
			if (error != null)
			{
				inputCssClass += " is-invalid";
				message = error.ErrorMessage;
				messageTextClass = "text-danger";
				labelTextClass = "text-danger";
			}
			RequiredAttribute requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
			if (requiredAttribute != null)
			{
				attributes += $"data-val-required=\"{requiredAttribute.ErrorMessage}\"";
			}


			//Gestione Label
			string labelName = property.Name;
			DisplayNameAttribute displayName = property.GetCustomAttribute<DisplayNameAttribute>();
			if (displayName != null)
			{
				labelName = displayName.DisplayName;
			}
			if (showRequiredOnLabel && requiredAttribute != null)
			{
				labelName += "*";

			}
			if (!string.IsNullOrEmpty(label))
			{
				labelName = label;
			}

			//Gestione calendario
			bool isDate = false;
			if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
			{
				isDate = true;
				inputCssClass += " it-date-datepicker";
			}
			//Gestione Orario
			bool isTime = false;
			TimeAttribute timeAttribute = property.GetCustomAttribute<TimeAttribute>();
			if (timeAttribute != null)
			{
				isTime = true;
			}
			//Gestione Checkbox
			bool isBoolean = false;
			if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
			{
				isBoolean = true;
			}
			//Gestione File
			bool isFile = false;
			if (typeof(IFormFile).IsAssignableFrom(property.PropertyType))
			{
				isFile = true;
			}
			//Gestione Documento
			bool isDocumento = false;
			Documento documento=null;
			if (property.PropertyType == typeof(Documento))
			{
				documento = rawValue as Documento;
				isDocumento = true;
			}

			//Gestione Length
			LengthAttribute lengthAttribute = property.GetCustomAttribute<LengthAttribute>();
			if (lengthAttribute != null && lengthAttribute.MaxLength > 0)
			{
				attributes += $" maxlength=\"{lengthAttribute.MaxLength}\" ";
			}

			//Gestione tooltip
			string tooltipText = "";
			if (!string.IsNullOrEmpty(tooltip))
			{
				tooltipText = $" data-toggle=\"tooltip\" title=\"{tooltip}\" ";
			}

			StringBuilder html = new StringBuilder();
			if (isTime)
			{
				if (placeholderAttribute == null)
					attributes+= " placeholder=\"hh:mm\"";
				html.Append($"<div class=\"form-group\">");
				html.Append($"<div class=\"input-group\">");
				html.Append($"<label id=\"{property.Name}_Label\" for=\"{property.Name}\" class=\"{labelTextClass}\">{labelName}</label>");
				html.Append($"<input {attributes} type=\"text\" class=\"input-time {inputCssClass}\" name=\"{property.Name}\" id=\"{property.Name}\"{hint} value=\"{value}\">");
				html.Append($"<div class=\"input-group-append\">");
				html.Append($"<div class=\"input-group-text\"><svg class=\"icon icon-sm\"><use xlink:href=\"/bootstrap-italia/dist/svg/sprite.svg#it-clock\"></use></svg></div>");
				html.Append($"</div>");
				html.Append($"</div>");
				html.Append($"<small class=\"form-text {messageTextClass}\" id=\"{property.Name}_Message\">{message}</small>");
				html.Append($"</div>");
			}
			else if (isBoolean)
			{
				//Chackbox Semplice (non ammette valori null)
				string checkedText = "";
				if (value?.ToLower() == "true")
				{
					checkedText = "checked=\"checked\" ";
				}
				html.Append($"<div class=\"form-check\" {tooltipText}>");
				html.Append($"<input {attributes} type=\"checkbox\" class=\"{inputCssClass}\" name=\"{property.Name}\" id=\"{property.Name}\"{hint} {checkedText} value=\"true\">");
				html.Append($"<label id=\"{property.Name}_Label\" for=\"{property.Name}\" class=\"{labelTextClass}\">{labelName}</label>");
				html.Append($"<small class=\"form-text {messageTextClass}\" id=\"{property.Name}_Message\">{message}</small>");
				html.Append("</div>");
			}else if(isFile){
				//html.Append($"<div class=\"form-group file-group {cssClass}\">");
				html.Append($"<input type=\"file\" name=\"{property.Name}\" id=\"{property.Name}\" class=\"upload\"/>");
				html.Append($"<label id=\"{property.Name}_Label\" for=\"{property.Name}\">");
				html.Append($"<svg class=\"icon icon-sm \" aria-hidden=\"true\"><use xlink:href=\"/bootstrap-italia/dist/svg/sprite.svg#it-upload\"></use></svg>");
				html.Append($"<span>{labelName}</span>");
				html.Append($"</label>");
				html.Append($"<div id=\"{property.Name}_Filename\" class=\"file-name\"></div>");
				html.Append($"<small class=\"form-text {messageTextClass}\" id=\"{property.Name}_Message\">{message}</small>");
				//html.Append("</div>");
			}
			else if (isDocumento)
			{
				string blob = "";
				if (documento?.Blob!=null && documento.Blob.Length>0)
				{
					blob = Convert.ToBase64String(documento.Blob);
				}
				html.Append($"<div class=\"form-group file-group {cssClass}\">");
				html.Append($"<input type=\"hidden\" id=\"{property.Name}.NomeFile\" name=\"{property.Name}.NomeFile\" value=\"{documento?.NomeFile}\"/>");
				html.Append($"<input type=\"file\" name=\"{property.Name}.File\" id=\"{property.Name}.File\" class=\"upload\"/>");
				html.Append($"<label id=\"{property.Name}.File_Label\" for=\"{property.Name}.File\">");
				html.Append($"<svg class=\"icon icon-sm \" aria-hidden=\"true\"><use xlink:href=\"/bootstrap-italia/dist/svg/sprite.svg#it-upload\"></use></svg>");
				html.Append($"<span>{labelName}</span>");
				html.Append($"</label>");
				if (blob.Length>0)
				{
					html.Append($"<button title=\"Elimina il file allegato\" alt=\"Elimina il file allegato\" class=\"btn btn-icon\" type=\"submit\" name=\"{property.Name}.Delete\" value=\"true\"><svg class=\"icon icon-sm icon-danger\"><use xlink:href=\"/bootstrap-italia/dist/svg/sprite.svg#it-close-big\"></use></svg></button>");
					html.Append($"<button title=\"Scarica il file allegato\" alt=\"Scarica il file allegato\" class=\"btn btn-icon\" type=\"submit\" name=\"{property.Name}.Download\" value=\"true\"><svg class=\"icon icon-sm icon-primary\"><use xlink:href=\"/bootstrap-italia/dist/svg/sprite.svg#it-download\"></use></svg></button>");
				}
				html.Append($"<div id=\"{property.Name}.File_Filename\" class=\"file-name\">{documento?.NomeFile}</div>");
				html.Append($"<small class=\"form-text {messageTextClass}\" id=\"{property.Name}.File_Message\">{message}</small>");
				if (blob.Length > 0 && string.IsNullOrEmpty(message))
				{
					html.Append($"<small class=\"form-text\" id=\"{property.Name}.Hash\">Hash: {documento.Hash}</small>");
				}
				html.Append($"</div>");
				//html.Append("</div>");
			}
			else
			{

				if (isDate)
					html.Append($"<div class=\"it-datepicker-wrapper theme-dark {cssClass}\"><div class=\"form-group\">");
				else
					html.Append($"<div class=\"form-group {cssClass}\">");
				html.Append($"<label id=\"{property.Name}_Label\" for=\"{property.Name}\" class=\"{labelTextClass}\">{labelName}</label>");
				html.Append($"<input {attributes} type=\"text\" class=\"{inputCssClass}\" name=\"{property.Name}\" id=\"{property.Name}\"{hint} value=\"{value}\">");
				html.Append($"<small class=\"form-text {messageTextClass}\" id=\"{property.Name}_Message\">{message}</small>");
				html.Append("</div>");
				if (isDate)
					html.Append($"</div>");
			}
			return new HtmlString(html.ToString());
		}




		public static IHtmlContent InputFor<TModel, TResult>(
			this IHtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TResult>> propertyLambda,
			string cssClass = null,
			bool showRequiredOnLabel = true,
			bool disabled = false,
			bool readOnly = false,
			string label = null,
			string tooltip = null)
		{
			return InputFor(
				htmlHelper,
				htmlHelper.ViewData.Model,
				propertyLambda,
				cssClass,
				showRequiredOnLabel,
				disabled,
				readOnly,
				label,
				tooltip
				);
		}

		public static IHtmlContent ComboboxFor<TModel, TResult>(
			this IHtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TResult>> propertyLambda,
			Dictionary<string, string> list = null,
			bool ricerca = false,
			string cssClass = null,
			bool disabled = false,
			bool showRequiredOnLabel = true)
		{
			Type type = typeof(TModel);
			// Recupero proietà richiesta
			if (!(propertyLambda.Body is MemberExpression member))
				throw new ArgumentException($"L'espressione '{propertyLambda}' si riferisce ad un metodo e non ad una proprietà.");

			PropertyInfo property = member.Member as PropertyInfo;
			if (property == null)
				throw new ArgumentException($"L'espressione '{propertyLambda}' si riferisce ad un campo e non ad una proprietà.");

			if (type != property.ReflectedType &&
				!type.IsSubclassOf(property.ReflectedType))
				throw new ArgumentException($"L'espressione '{propertyLambda}' si riferisca ad una proprietà che non è del tipo {type}.");

			if (cssClass.Contains('\"'))
			{
				throw new ArgumentException($"La classe CSS non deve contenere caratteri \"");
			}

			if (list == null)
			{
				if (htmlHelper.ViewData[property.Name] == null)
				{
					throw new ArgumentException($"Per la comboBox della proprietà {property.Name} non è definita la lista, valorizzare il parametro list o inserire il dictionary nella Viewdata[\"{property.Name}\"]");
				}
				if (!(htmlHelper.ViewData[property.Name] is Dictionary<string, string>))
				{
					throw new ArgumentException($"Per la comboBox della proprietà {property.Name} la Viewdata[\"{property.Name}\"] deve essere del tipo Dictionary<string,string>");

				}
				list = htmlHelper.ViewData[property.Name] as Dictionary<string, string>;
			}
			DateAttribute dateAttribute = property.GetCustomAttribute<DateAttribute>();
			//Recupero Dati
			string value = "";
			try
			{
				if (htmlHelper.ViewData.Model != null)
				{

					PropertyInfo propInfo = htmlHelper.ViewData.Model.GetType().GetProperty(property.Name);
					if (propInfo != null)
					{
						if (dateAttribute != null && (propInfo.PropertyType == typeof(DateTime) || propInfo.PropertyType == typeof(DateTime?)))
						{
							value = ((DateTime?)propInfo.GetValue(htmlHelper.ViewData.Model))?.ToString("dd/MM/yyyy");
						}
						else
						{
							value = propInfo.GetValue(htmlHelper.ViewData.Model)?.ToString();
						}
					}
				}
				/*else
				{
					HttpRequest request = htmlHelper.ViewContext?.HttpContext?.Request;
					if (request != null)
					{
						value = request.Form[property.Name];
					}
				}*/

			}
			catch (Exception e)
			{
				//Se ci sono errori assumo che non ci sia il valore
			}
			string messageTextClass = "text-muted";
			string labelTextClass = "";
			string message = "";
			string hint = "";
			HintAttribute hintAttribute = property.GetCustomAttribute<HintAttribute>();
			if (hintAttribute != null)
			{
				message = hintAttribute.Text;
				hint = $" data-val-hint=\"{message}\"";
			}
			string selectProprieties = "";
			string wrapperProprieties = "";
			PlaceholderAttribute placeholderAttribute = property.GetCustomAttribute<PlaceholderAttribute>();
			if (placeholderAttribute != null)
			{
				selectProprieties = $"title=\"{placeholderAttribute.Text}\"";
			}
			//Gestione Errori
			var error = htmlHelper.ViewData?.ModelState[property.Name]?.Errors.FirstOrDefault();
			if (error != null)
			{
				message = error.ErrorMessage;
				messageTextClass = "text-danger";
				labelTextClass = "text-danger";
				cssClass += " is-invalid";
			}
			//Gestione Disabilitazione
			if (disabled)
			{
				selectProprieties += " disabled";
				wrapperProprieties += " disabled";
			}

			//Gestione Label

			string labelName = property.Name;
			DisplayNameAttribute displayName = property.GetCustomAttribute<DisplayNameAttribute>();
			if (displayName != null)
			{
				labelName = displayName.DisplayName;
			}
			if (showRequiredOnLabel && property.GetCustomAttribute<RequiredAttribute>() != null)
			{
				labelName += "*";

			}

			//Gestione ricerca
			string ricercaText = ricerca ? "data-live-search=\"true\"" : "";

			//Costruzione html
			StringBuilder html = new StringBuilder();
			html.Append($"<div id=\"{property.Name}_Wrapper\" class=\"bootstrap-select-wrapper {cssClass}\"{wrapperProprieties}>");
			html.Append($"<label for=\"{property.Name}\" class=\"{labelTextClass}\">{labelName}</label>");
			html.Append($"<select {selectProprieties} {ricercaText} name=\"{property.Name}\" id=\"{property.Name}\"{hint}>");
			html.Append($"<option></option>");
			foreach (var item in list)
			{
				if (item.Key == value)
				{
					html.Append($"<option  selected value=\"{item.Key}\">{item.Value}</option>");
				}
				else
				{
					html.Append($"<option value=\"{item.Key}\">{item.Value}</option>");
				}
			}
			html.Append($"</select>");
			html.Append($"<small class=\"form-text {messageTextClass}\" id=\"{property.Name}_Message\">{message}</small>");
			html.Append("</div>");
			return new HtmlString(html.ToString());
		}

		/// <summary>
		/// Visualizza un sommario di tutti gli errori presenti nel Modelstate.
		/// Il sommario visulizzerà il nome del campo che ha causato gli errori e di seguito l'elenco di errori
		/// Il testo del campo sarà cliccabile e metterà il focus al campo di input.
		/// </summary>
		/// <typeparam name="TModel">Model di riferimento</typeparam>
		/// <param name="htmlHelper">html di riferimento</param>
		/// <param name="message">Messaggio da visualizzare nella prima riga del sommario</param>
		/// <returns></returns>
		public static IHtmlContent Sommario<TModel>(this IHtmlHelper<TModel> htmlHelper, string message = "Si sono verificati i seguenti errori:")
		{
			StringBuilder html = new StringBuilder();
			if (!htmlHelper.ViewData.ModelState.IsValid)
			{
				html.Append("<div class=\"alert alert-danger alert-dismissible fade show text-danger\" role=\"alert\">");
				html.Append(message);
				html.Append("<ul class=\"alert-list\">");
				foreach (var key in htmlHelper.ViewData.ModelState.Keys)
				{
					var propieta = htmlHelper.ViewData.ModelState[key];

					int numeroErrori = propieta.Errors.Count();
					if (numeroErrori > 0)
					{
						var nome = key;
						PropertyInfo property = typeof(TModel).GetProperty(key);
						if (property != null)
						{
							SummaryAttribute summaryAttribute = property.GetCustomAttribute<SummaryAttribute>();
							if (summaryAttribute != null)
							{
								nome = summaryAttribute.Text;
							}
							else
							{
								DisplayNameAttribute attribute = property.GetCustomAttribute<DisplayNameAttribute>();
								if (attribute != null)
								{
									nome = attribute.DisplayName;
								}
							}
						}

						html.Append("<li>");
						if (numeroErrori == 1)
						{
							html.Append($"<strong><a class=\"list-item\" href=\"#{key}\">{nome}</a>:</strong> {propieta.Errors[0].ErrorMessage}");
						}
						else
						{
							html.Append($"<strong><a class=\"list-item\" href=\"#{key}\">{nome}</a>:</strong>");
							html.Append("<ul>");
							foreach (var error in propieta.Errors)
							{

								html.Append("<li>");

								html.Append($"{error.ErrorMessage}");
								html.Append("</li>");

							}
							html.Append("</ul>");
						}
						html.Append("</li>");
					}
				}
				html.Append("</ul>");

				html.Append("</div>");
			}
			return new HtmlString(html.ToString());
		}

		public static IHtmlContent Sommario<TModel>(this IHtmlHelper htmlHelper,TModel model, string message = "Si sono verificati i seguenti errori:")
		{
			StringBuilder html = new StringBuilder();
			if (!htmlHelper.ViewData.ModelState.IsValid)
			{
				html.Append("<div class=\"summary alert alert-danger alert-dismissible fade show text-danger\" role=\"alert\">");
				html.Append(message);
				html.Append("<ul class=\"alert-list\">");
				foreach (var key in htmlHelper.ViewData.ModelState.Keys)
				{
					var propieta = htmlHelper.ViewData.ModelState[key];

					int numeroErrori = propieta.Errors.Count();
					if (numeroErrori > 0)
					{
						var nome = key;
						PropertyInfo property = typeof(TModel).GetProperty(key);
						if (property != null)
						{
							SummaryAttribute summaryAttribute = property.GetCustomAttribute<SummaryAttribute>();
							if (summaryAttribute != null)
							{
								nome = summaryAttribute.Text;
							}
							else
							{
								DisplayNameAttribute attribute = property.GetCustomAttribute<DisplayNameAttribute>();
								if (attribute != null)
								{
									nome = attribute.DisplayName;
								}
							}
						}

						html.Append("<li>");
						if (numeroErrori == 1)
						{
							html.Append($"<strong><a class=\"list-item\" href=\"#{key}\">{nome}</a>:</strong> {propieta.Errors[0].ErrorMessage}");
						}
						else
						{
							html.Append($"<strong><a class=\"list-item\" href=\"#{key}\">{nome}</a>:</strong>");
							html.Append("<ul>");
							foreach (var error in propieta.Errors)
							{

								html.Append("<li>");

								html.Append($"{error.ErrorMessage}");
								html.Append("</li>");

							}
							html.Append("</ul>");
						}
						html.Append("</li>");
					}
				}
				html.Append("</ul>");

				html.Append("</div>");
			}
			return new HtmlString(html.ToString());
		}


		public static IHtmlContent Grid<Entity>(this IHtmlHelper htmlHelper, ResultList<Entity> list, GridOptions options = null)
		{
			if (list == null)
			{
				return new HtmlString("<p>Errore nella griglia<p>");
			}
			if (options == null)
			{
				options = new GridOptions();
			}
			if (!string.IsNullOrWhiteSpace(options.EmptyText) && list.Count == 0)
			{
				string emptyText = HttpUtility.HtmlEncode(options.EmptyText);
				return new HtmlString($"<div class='table-empty'>{emptyText}</div>");
			}
			string ajaxPost = options.AjaxPost ? "ajaxForm" : "";
			/**Gestione totali **/
			string risultati = null;
			if (options.ShowResults)
				risultati = $"<div>Pagina {list.Page} di {list.PageCount} ({list.Count} elementi trovati) </div>";
			string paginazione = null;
			/** Gestione Paginazione **/
			if (options.Paginator != GridPaginatorType.None && list.PageCount > 1)
			{
				int paginaIniziale = list.Page - 5;
				int paginaFinale = list.Page + 5;
				if (paginaIniziale < 1) { paginaIniziale = 1; }
				if (paginaFinale > list.PageCount) { paginaFinale = list.PageCount; }
				paginazione = "<div class='container'><nav class='text-center'><ul class='pagination'>";
				if (list.Page == 1)
				{
					paginazione += "<li class='page-item disabled'><span class='page-link' href='#'>Precedente</span></li>";
				}
				else
				{
					paginazione += $"<li class='page-item'><button class='page-link {ajaxPost}' type='submit' name='Page' value='{list.Page - 1}'>Precedente</button></li>";
				}
				for (int i = paginaIniziale; i <= paginaFinale; i++)
				{
					if (i == list.Page)
					{
						paginazione += $"<li class='page-item active'><span class='page-link'>{i}</span></li>";
					}
					else
					{
						paginazione += $"<li class='page-item'><button class='page-link {ajaxPost}' type='submit' name='Page' value='{i}'>{i}</button></li>";
					}
				}
				if (list.Page >= list.PageCount)
				{
					paginazione += $"<li class='page-item disabled'><span class='page-link'>Successiva</span></li>";
				}
				else
				{
					paginazione += $"<li class='page-item'><button class='page-link {ajaxPost}' type='submit' name='Page' value='{list.Page + 1}'>Successiva</button></li>";
				}
				paginazione += "</ul></nav></div>";
			}
			List<Column> columns = null;
			if (!string.IsNullOrWhiteSpace(options.ColumnNames))
			{
				columns = GenerateColumns(typeof(Entity), options.ColumnNames, options.CustomColumns);
			}
			else if (options.CustomColumns != null)
			{
				columns = options.CustomColumns;
			}
			else
			{
				columns = GenerateColumns(typeof(Entity));
			}
			bool ascending = true;
			string order = null;
			if (!string.IsNullOrEmpty(list.Order))
			{
				if (list.Order.EndsWith(" ASC", System.StringComparison.OrdinalIgnoreCase))
				{
					ascending = true;
					order = list.Order.Remove(list.Order.Length - 4);
				}
				if (list.Order.EndsWith(" DESC", System.StringComparison.OrdinalIgnoreCase))
				{
					ascending = false;
					order = list.Order.Remove(list.Order.Length - 5);
				}
			}
			StringBuilder html = new StringBuilder();

			html.Append(risultati);
			if (options.Paginator == GridPaginatorType.Top || options.Paginator == GridPaginatorType.Both)
			{
				html.Append(paginazione);
			}
			html.Append($"<input type='hidden' name=CurrentPage value='{list.Page}'>");
			html.Append($"<input type='hidden' name=CurrentOrder value='{list.Order}'>");
			if (string.IsNullOrWhiteSpace(options.GridId))
			{
				html.Append("<div class=\"table-responsive\"><table class='table grid'><thead><tr>");
			}
			else
			{
				html.Append($"<div class=\"table-responsive\"><table class='table grid' id='{options.GridId}'><thead><tr>");
			}
			/* Creazione intestazioni di colonna*/
			foreach (var column in columns)
			{
				string headerName = HttpUtility.HtmlEncode(column.HeaderName);
				string orderBy = column.OrderBy ?? column.Name;
				if (!column.Orderable || (!options.OrderColumns))
				{
					html.Append($"<th scope='col'>{headerName}</button></th>");
				}
				else if (orderBy == order)
				{
					if (ascending)
					{
						html.Append($"<th scope='col'><button type='submit' class='btn btn-link {ajaxPost}' name='OrderColumns' value='{orderBy} DESC'>{headerName} <i class='fas fa-caret-up'></i></button></th>");
					}
					else
					{
						html.Append($"<th scope='col'><button type='submit' class='btn btn-link {ajaxPost}' name='OrderColumns' value='default'>{headerName} <i class='fas fa-caret-down'></i></button></th>");
					}
				}
				else
				{
					html.Append($"<th scope='col'><button type='submit' class='btn btn-link {ajaxPost}' name='OrderColumns' value='{orderBy} ASC'>{headerName}</button></th>");
				}
			}
			html.Append("</tr></thead><tbody>");
			foreach (Entity item in list.List)
			{
				if (options.CustomRowClass == null)
				{
					html.Append("<tr>");
				}
				else
				{
					string customClass = options.CustomRowClass(item);
					if (string.IsNullOrWhiteSpace(customClass))
					{
						html.Append("<tr>");
					}
					else
					{
						html.Append($"<tr class=\"{customClass}\">");
					}
				}
				foreach (var column in columns)
				{
					if (column.CustomResult == null)
					{
						var value = GetPropertyValue(item, column.Name);
						string text;
						if (value != null &&
							(column.Property.PropertyType == typeof(DateTime) || column.Property.PropertyType == typeof(DateTime?)) &&
							column.Property.GetCustomAttribute<DateAttribute>() != null)
						{
							DateAttribute attribute = column.Property.GetCustomAttribute<DateAttribute>();
							text = ((DateTime)value).ToString(attribute.Format);
						}
						else if (value != null &&
						   (column.Property.PropertyType == typeof(decimal) || column.Property.PropertyType == typeof(decimal?)) &&
						   column.Property.GetCustomAttribute<MoneyAttribute>() != null)
						{
							MoneyAttribute attribute = column.Property.GetCustomAttribute<MoneyAttribute>();
							text = ((decimal)value).ToString($"{attribute.Currency} {attribute.Format}");
						}
						else if (value != null &&
						  (column.Property.PropertyType == typeof(long) || column.Property.PropertyType == typeof(long?)) &&
						  column.Property.GetCustomAttribute<FileSizeAttribute>() != null)
						{
							FileSizeAttribute attribute = column.Property.GetCustomAttribute<FileSizeAttribute>();
							text = FileSize.FormatToString((long)value);
						}
						else if (value != null &&
						   (column.Property.PropertyType == typeof(bool) || column.Property.PropertyType == typeof(bool?)))
						{
							bool? booleanValue = (bool?)value;
							DisplayBooleanAttribute attribute = column.Property.GetCustomAttribute<DisplayBooleanAttribute>();
							if (attribute != null && booleanValue.HasValue)
							{
								if (booleanValue.Value)
								{
									text = attribute.TrueText;
								}
								else
								{
									text = attribute.FalseText;
								}
							}
							else
							{
								text = value?.ToString();
							}
						}
						else
						{
							text = value?.ToString();
						}
						html.Append($"<td>{HttpUtility.HtmlEncode(text)?.Replace("\n", "<br/>")}</td>");
					}
					else
					{
						html.Append($"<td>{column.CustomResult(item)}</td>");
					}
				}
				html.Append("</tr>");
			}
			html.Append("</tbody></table></div>");
			if (options.Paginator == GridPaginatorType.Bottom || options.Paginator == GridPaginatorType.Both)
			{
				html.Append(paginazione);
			}

			return new HtmlString(html.ToString());
		}

		public static Column GenerateColumn(PropertyInfo property)
		{
			bool orderable = true;
			GridOptionsAttribute gridOptions = property.GetCustomAttribute<GridOptionsAttribute>();
			if (gridOptions != null)
			{
				if (!gridOptions.Visible)
				{
					return null;
				}
				orderable = gridOptions.Orderable;
			}
			if (property.PropertyType != typeof(string) && property.PropertyType.GetInterfaces().Any(
							i => i.IsGenericType &&
							i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
			{
				return null;
			}

			string columnName = property.Name;
			DisplayNameAttribute displayName = property.GetCustomAttribute<DisplayNameAttribute>();
			if (displayName != null)
			{
				columnName = displayName.DisplayName;
			}
			string orderBy = null;
			OrderByAttribute orderByAttribute = property.GetCustomAttribute<OrderByAttribute>();
			if (orderByAttribute != null)
			{
				orderBy = orderByAttribute.ColumnNames;
			}

			Column column = new Column()
			{
				Name = property.Name,
				Type = property.GetType(),
				HeaderName = columnName,
				Property = property,
				Orderable = orderable,
				OrderBy = orderBy ?? gridOptions?.OrderBy
			};
			return column;
		}

		public static List<Column> GenerateColumns(Type type, string list, List<Column> customColumns)
		{
			List<Column> columns = new List<Column>();
			if (customColumns == null)
				customColumns = new List<Column>();

			foreach (string columnName in list.Split(","))
			{

				var customColumn = customColumns.FirstOrDefault(x => x.Name == columnName);
				if (customColumn != null)
				{
					columns.Add(customColumn);
					continue;
				}
				PropertyInfo columnProperty = GetPropertyInfo(type, columnName);
				if (columnProperty == null)
				{
					throw new Exception($"Proprietà [{columnName}] non trovata");
				}
				Column column = GenerateColumn(columnProperty);
				column.Name = columnName;
				if (column != null)
				{
					columns.Add(column);
				}
			}
			return columns;
		}


		public static List<Column> GenerateColumns(Type type)
		{
			List<Column> columns = new List<Column>();

			foreach (PropertyInfo property in type.GetProperties())
			{
				GridOptionsAttribute gridOptions = property.GetCustomAttribute<GridOptionsAttribute>();
				if (gridOptions != null && !gridOptions.Visible)
				{
					continue;
				}
				if (gridOptions != null && gridOptions.ShowChilds)
				{
					List<Column> subColumns = GenerateColumns(property.PropertyType);
					subColumns.ForEach(x => x.Name = $"{property.Name}.{x.Name}");
					columns.AddRange(subColumns);

				}
				else
				{
					Column column = GenerateColumn(property);
					if (column != null)
					{
						columns.Add(column);
					}
				}
			}
			return columns;
		}

		public static object GetPropertyValue(object src, string propName)
		{
			if (src == null) return null;
			if (propName == null) return null;

			if (propName.Contains("."))
			{
				var temp = propName.Split(new char[] { '.' }, 2);
				return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
			}
			else
			{
				var prop = src.GetType().GetProperty(propName);
				return prop != null ? prop.GetValue(src, null) : null;
			}
		}
		public static PropertyInfo GetPropertyInfo(Type src, string propName)
		{
			if (src == null) return null;
			if (propName == null) return null;

			if (propName.Contains("."))
			{
				var temp = propName.Split(new char[] { '.' }, 2);
				return GetPropertyInfo(src.GetProperty(temp[0]).PropertyType, temp[1]);
			}
			else
			{
				return src.GetProperty(propName);
			}
		}

		public class GridOptionsAttribute : Attribute
		{
			public bool Orderable { get; set; } = true;
			public string OrderBy { get; set; }
			public bool Visible { get; set; } = true;
			public bool ShowChilds { get; set; } = false;

		}
		


	}
}
