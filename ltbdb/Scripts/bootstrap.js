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
			success: function (data) {
				if (data === "")
					jbox_tag.close();
				else
					$('.jBox-content').html(data);
			},
			cache: false
		});
	});
});