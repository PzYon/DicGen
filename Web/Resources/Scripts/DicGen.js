var DicGenApp = angular.module("DicGenApp", ["ngAnimate"]);

var DicGenController = DicGenApp.controller("DicGenController", function($scope, repository)
{
	$scope.Generate = function()
	{
		$scope.IsLoading = true;

		repository.Generate({ Text: $scope.Text, SortOrder: $scope.SortOrder }, function (items)
		{
			$scope.Items = items;
			$scope.WordCount = items.length;
			$scope.Render();

			$scope.IsLoading = false;
		});
	};

	$scope.Render = function()
	{
		var separator = $scope.ShowOnOneLine ? " " : "\n";
		$scope.Text = "";
		angular.forEach($scope.Items, function (item) { $scope.Text += item + separator; });
	};

	$scope.Initialize = function ()
	{
		$scope.SortOrder = "Alphabetically";
	};

	$scope.Initialize();
});

DicGenApp.directive("setBackground", function()
{
	return {
		restrict: "A",
		link: function(scope, element, attributes, ctrl)
		{
			scope.$watch(function() { return scope.WordCount; },
			             function(value) { SetBackground((value || "").toString());});

			function SetBackground(value)
			{
				if (isNaN(value) || value.length == 0)
				{
					return;
				}

				var index = value.length - 1;
				var offsetRight = 25;
				var images = [];

				while (index >= 0)
				{
					images.push({ Number: value.charAt(index--), OffsetRight: offsetRight });
					offsetRight += 22;
				}

				var urls = "";
				var positions = "";

				for (var i = images.length - 1; i >= 0; i--)
				{
					var image = images[i];

					urls += "url('/Resources/Images/Numbers/" + image.Number + ".png')";
					positions += "calc(100% - " + image.OffsetRight + "px) -2px";

					if (i > 0)
					{
						urls += ", ";
						positions += ", ";
					}
				}

				element.attr("style", "background-image:" + urls + "; background-position: " + positions + "; background-repeat: no-repeat;");
			}
		}
	};
});

DicGenApp.directive("typeText", function($interval)
{
	return {
		restrict: "A",
		link: function(scope, element, attributes, ctrl)
		{
			element.bind("mousedown", UnregisterDirective);
			var typist = TypeText(attributes.typeText);

			function TypeText(textToType)
			{
				scope.Text = "";

				var length = textToType.length;
				var typedChars = 0;

				return $interval(function()
				{
					scope.Text += textToType.charAt(typedChars++);

					if (typedChars == length)
					{
						$interval.cancel(typist);
					}
				}, 100);
			}

			function UnregisterDirective()
			{
				element.val("");
				element.unbind("mousedown", UnregisterDirective);
				$interval.cancel(typist);
			}
		}
	};
});

DicGenApp.directive("fillUp", function()
{
	return {
		restrict: "A",
		link: function(scope, element, attributes, ctrl)
		{
			var someBottomMargin = 30;
			var targetHeight = window.innerHeight - element.prop("offsetTop") - someBottomMargin;
			element.css("height", targetHeight + "px");
		}
	};
});

DicGenApp.factory("repository", function($http, ApiUrl)
{
	function CallServer(url, method, postData, success)
	{
		$http({ url: url, method: method, data: postData })
			.success(function(data, status, headers, config)
			{
				success(data);
			})
			.error(function(data, status, headers, config)
			{
				alert(status + ": " + data.Message);
			});
	}

	return {
		Generate: function(data, success)
		{
			CallServer(ApiUrl + "generator/generate", "post", data, success);
		},
	};
});