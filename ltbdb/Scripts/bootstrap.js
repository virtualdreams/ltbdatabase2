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
		source: '/ac-search',
		minLength: 3,
		select: function (event, ui) {
			if (ui.item) {
				$(event.target).val(ui.item.value);
			}
			$(event.target.form).submit();
		}
	});

	$("#t").autocomplete({
		source: '/ac-tag',
		minLength: 3,
		select: function (event, ui) {
			if (ui.item) {
				$(event.target).val(ui.item.value);
			}
			$(event.target.form).submit();
		}
	});

	/* TEST */
	jbox_tag = new jBox('Modal', {
		position: {
			x: 'left',
			y: 'top'
		},
		offset: {
			y: 25
		}
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

	$(document).on('submit', '#tag-form', function(e) {
		e.preventDefault();
		$.ajax({
			url: this.action,
			type: 'POST',
			data: $('#tag-form').serialize(),
			success: function(response, status, xhr) {
				var ct = xhr.getResponseHeader("content-type") || "";
				if (ct.indexOf('html') > -1) {
					//html
					$('.jBox-content').html(response);
				}
				if (ct.indexOf('json') > -1) {
					var template = '<div class="tag" style="position: relative;">\
						<a class="tag-remove" href="/tag/unlink/{1}?bookid={2}" title="Tag entfernen.">&nbsp;</a>\
						<a href="/tag/view/{1}" title="Referenzen: {3}">{0}</a>\
						</div>';

					$.each(response.tags, function () {
						var t = template.replace(/\{0\}/g, this.Name).replace(/\{1\}/g, this.Id).replace(/\{2\}/g, response.bookid).replace(/\{3\}/g, this.References);
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
			success: function (response, status, xhr) {
				var ct = xhr.getResponseHeader("content-type") || "";
				if (ct.indexOf('json') > -1) {
					if (response.Success)
						$(p).remove();
					else
						alert("Can't remove tag.")
				}
			},
			cache: false
		});
	});
});