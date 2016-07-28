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
	return val.split(/;\s*/);
}

function extractLast(term) {
	return split(term).pop();
}

$(function () {
	/* autocomplete for search */
	$('#q').autocomplete({
		source: '/api/search/title',
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

	/* autocomplete for tags */
	$('#t').autocomplete({
		source: function (request, response) {
			$.getJSON('/api/search/tags', {
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
			this.value = terms.join("; ");
			return false;
		},
		open: function () {
			$(this).autocomplete("widget").width($(this).outerWidth() - 6);
		}
	});

	/* image box */
	jbox_image = new jBox('Image');

	/* tag message box */
	jbox_tag = new jBox('Modal', {
		position: {
			x: 'left',
			y: 'top'
		},
		offset: {
			y: 25
		},
		attach: $('#addtag'),
		target: $('#addtag'),
		content: $('#tag-msg-box'),
		overlay: false,
		zIndex: 400,
		closeOnClick: 'overlay'
	});

	var tag_template = '<div class="tag" style="position: relative;">\
								<span class="tag-remove" data-id="{1}" data-tagid="{2}"><i class="material-icons bigger" title="Dieses Tag entfernen.">delete</i></span>\
								<a href="/tag/{0}" title="">{0}</a>\
							</div>';

	$('#add-button').click(function (e) {
		data = {
			id: $('#t').attr('data-id'),
			tags: $('#t').val()
		};

		$.ajax({
			cache: false,
			url: '/api/tag/add',
			type: 'POST',
			data: data,
			dataType: 'json',
			statusCode: {
				403: function () {
					location.href = '/account/login?ReturnUrl=' + encodeURIComponent(location.pathname);
				},
				404: function () {
					alert('Resource not found.');
				}
			},
			success: function (data) {
				$.each(data, function (i, item) {
					var t = tag_template.replace(/\{0\}/g, item.Name).replace(/\{1\}/g, item.Book).replace(/\{2\}/g, item.Id);
					$(t).insertBefore('#tag-add');
				});
				jbox_tag.close();
			}
		});
	});

	/* remove a tag */
	$(document).on('click', '.tag-remove', function (e) {
		var p = $(this).parent();

		data = {
			id: $(this).attr('data-id'),
			tagid: $(this).attr('data-tagid')
		};

		$.ajax({
			cache: false,
			url: '/api/tag/remove',
			type: 'POST',
			data: data,
			dataType: 'json',
			statusCode: {
				403: function () {
					location.href = '/account/login?ReturnUrl=' + encodeURIComponent(location.pathname);
				},
				404: function () {
					alert("Resource not found.");
				}
			},
			success: function(data){
				if (data.success) {
					$(p).remove();
				} else {
					alert("Dieses Tag konnte nicht entfernt werden.");
				}
			}
		});
	});

	/* story management */
	var story_container = $('#story-container');
	var story_template =	'<div class="form-element-field story">\
								<input type="text" name="stories" value="" placeholder="Inhalt" /> <input class="button-green story-ins" type="button" value="+" /> <input class="button-red story-rem" type="button" value="&ndash;" />\
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

	/* validation */
	$.validator.addMethod(
	    "regex",
	    function (value, element, regexp) {
	    	return this.optional(element) || value.match(regexp);
	    },
	    "Please check your input."
	);

	$.validator.addMethod(
		'nowhitespace',
		function (value, element) {
			return this.optional(element) || value.trim() != ''
		},
		'No white space.'
	);

	$('#book-form').validate({
		errorClass: 'field-validation-error',
		validClass: 'field-validation-valid',
		errorElement: 'span',
		rules: {
			number: {
				required: true,
				nowhitespace: true,
				regex: '[0-9]+'
			},
			name: {
				required: true,
				nowhitespace: true
			}
		},
		messages: {
			number: {
				required: 'Bitte gib eine Nummer ein.',
				nowhitespace: 'Bitte gib eine Nummer ein.',
				regex: 'Bitte gib eine Nummer ein.'
			},
			name: {
				required: 'Bitte gib einen Titel ein.',
				nowhitespace: 'Bitte gib einen Titel ein.'
			}
		}
	});

	$('#category-form').validate({
		errorClass: 'field-validation-error',
		validClass: 'field-validation-valid',
		errorElement: 'span',
		rules: {
			name: {
				required: true,
				nowhitespace: true
			}
		},
		messages: {
			name: {
				required: 'Bitte gib einen Namen ein.',
				nowhitespace: 'Bitte gib einen Namen ein.'
			}
		}
	});

	$('#tag-edit').validate({
		errorClass: 'field-validation-error',
		validClass: 'field-validation-valid',
		errorElement: 'span',
		rules: {
			name: {
				required: true,
				nowhitespace: true
			}
		},
		messages: {
			name: {
				required: 'Bitte gib einen Namen ein.',
				nowhitespace: 'Bitte gib einen Namen ein.'
			}
		}
	});

	$('#login-form').validate({
		errorClass: 'field-validation-error',
		validClass: 'field-validation-valid',
		errorElement: 'span',
		rules: {
			username: {
				required: true,
				nowhitespace: true
			},
			password: {
				required: true,
				nowhitespace: true
			}
		},
		messages: {
			username: {
				required: 'Bitte gib einen Benutzernamen ein.',
				nowhitespace: 'Bitte gib einen Benutzernamen ein.'
			},
			password: {
				required: 'Bitte gib ein Passwort ein.',
				nowhitespace: 'Bitte gib ein Passwort ein.'
			}
		}
	});
});