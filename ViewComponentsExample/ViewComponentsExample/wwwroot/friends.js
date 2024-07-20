document.querySelector("#load-friends-button").addEventListener("click",
    async function () {
        var response = await fetch("friends-list", { method: "GET" });
        var responseBody = await response.text();
        document.querySelector("#friends-list").innerHTML = responseBody;
    });