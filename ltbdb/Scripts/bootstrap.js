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

	jbox_tag = new jBox('Modal', {
		ajax: {
			url: '/addtag',
			reload: true,
			type: 'GET'
		},
		position: {
			x: 'left',
			y: 'top'
		},
		offset: {
			y: 25
		},
		target: $('#tag')
	});
});