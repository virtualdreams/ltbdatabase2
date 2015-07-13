$.widget("custom.catcomplete", $.ui.autocomplete, {
	_renderMenu: function(ul, items) {
		var that = this, currentCategory = "";
		$.each(items, function(index, item) {
			if(item.category != currentCategory) {
				ul.append('<li class="ui-autocomplete-category">' + item.category + '</li>');
				currentCategory = item.category;
			}
			that._renderItemData(ul, item);
		});
	}
});

function split(val) {
	return val.split(/,\s*/);
}

function extractLast(term) {
	return split(term).pop();
}

$(function() {
	$('.tt').tooltip({
		track: true
	});
	
	$(document).on('click', '.message-hidden-field', function() {
		$(this).remove();
	});
	
	$(document).on('mouseover', '.message-hidden-field', function() {
		$(this).tooltip();
	});

	$("#q").autocomplete({
		source: '/ac-search/',
		minLength: 3,
		select: function (event, ui) {
			if (ui.item) {
				$(event.target).val(ui.item.value);
			}
			$(event.target.form).submit();
		},
		open: function () {
			$(this).autocomplete("widget").width($(this).outerWidth() - 6);
		}
	});

	$(document).on("keydown.autocomplete", "#t", function (e) {
		$(this).bind("keydown", function (event) {
			if (event.keyCode === $.ui.keyCode.TAB && $(this).autocomplete("instance").menu.active) {
				event.preventDefault();
			}
		}).autocomplete({
			source: function (request, response) {
				$.getJSON('/ac-tag/', {
					term: extractLast(request.term)
				}, response);
			},
			search: function () {
				var term = extractLast(this.value);
				if (term.length < 3) {
					return false;
				}
			},
			focus: function () {
				return false;
			},
			select: function (event, ui) {
				var terms = split(this.value);
				terms.pop();
				terms.push(ui.item.value);
				terms.push("");
				this.value = terms.join(", ");
				return false;
			},
			open: function () {
				$(this).autocomplete("widget").width($(this).outerWidth() - 6);
			}
		})
	});

	/* TEST */
	jbox_tag = new jBox('Modal', {
		position: {
			x: 'left',
			y: 'top'
		},
		offset: {
			y: 25
		},
		overlay: false,
		zIndex: 400,
		closeOnClick: 'overlay'
	});

	$('.addtag').click(function (e) {
		e.preventDefault();
		jbox_tag.open({
			target: $(this)
		}).ajax({
			url: this.href,
			reload: true,
			type: 'GET'
		});
	});

	var tag_template = '<div class="tag" style="position: relative;">\
							<a class="tag-remove" href="/tag/unlink/{1}?bookid={2}" title="Tag entfernen."><img src="/Content/link_break.png" alt="" /></a>\
							<a href="/tag/view/{1}" title="Referenzen: {3}">{0}</a>\
						</div>';

	$(document).on('submit', '#tag-form', function(e) {
		e.preventDefault();
		$.ajax({
			url: this.action,
			type: 'POST',
			data: $('#tag-form').serialize(),
			statusCode: {
				403: function () {
					location.href = '/account/login?ReturnUrl=' + encodeURIComponent(location.pathname);
				}
			},
			success: function(response, status, xhr) {
				var ct = xhr.getResponseHeader("content-type") || "";
				if (ct.indexOf('html') > -1) {
					//html
					$('.jBox-content').html(response);
				}
				if (ct.indexOf('json') > -1) {
					$.each(response.tags, function () {
						var t = tag_template.replace(/\{0\}/g, this.Name).replace(/\{1\}/g, this.Id).replace(/\{2\}/g, response.bookid).replace(/\{3\}/g, this.References);
						$(t).insertBefore('#tag-add');
					});

					jbox_tag.close();
				}
			},
			cache: false
		});
	});

	$(document).on('click', '.tag-remove', function (e) {
		e.preventDefault();

		var p = $(this).parent();

		$.ajax({
			url: this.href,
			type: 'POST',
			statusCode: {
				403: function () {
					location.href = '/account/login?ReturnUrl=' + encodeURIComponent(location.pathname);
				}
			},
			success: function (response, status, xhr) {
				var ct = xhr.getResponseHeader("content-type") || "";
				if (ct.indexOf('json') > -1) {
					if (response.success)
						$(p).remove();
					else
						alert("Can't remove tag.")
				}
			},
			cache: false
		});
	});

	var story_container = $('#story-container');
	var story_template =	'<div class="form-element-field story">\
								<input type="text" name="stories" value="" placeholder="Inhalt" /> <input class="button-green story-ins" type="button" value="Einf&uuml;gen" /> <input class="button-red story-rem" type="button" value="Entfernen" />\
							</div>';

	$(document).on('click', '#story-add', function (e) {
		$(story_template).appendTo(story_container);
	});

	$(document).on('click', '.story-ins', function (e) {
		var p = $(this).parent();
		$(story_template).insertBefore(p);
	});

	$(document).on('click', '.story-rem', function (e) {
		var p = $(this).parent();
		p.remove();
	});

	$('#cancel-edit').click(function () {
		location.href = '/';
	});
});