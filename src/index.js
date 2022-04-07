require("dotenv").config();
const fs = require("fs");
const path = require("path");
const axios = require("axios");
const open = require("open");
const uuid = require("uuid");
const cheerio = require("cheerio");
const { USER_AGENTS, CHAR_LIST } = require("./contants");
const FETCH_IMAGE_COUNT = process.env.FETCH_IMAGE_COUNT;
const ARCHIVE_LOCATION = path.join(__dirname, "..", "archive");

function getRandomInt(max) {
  return Math.floor(Math.random() * max);
}

function getRandomUserAgent() {
  return USER_AGENTS[getRandomInt(USER_AGENTS.length)];
}

function generateScreenshotId() {
  let imgUrl = "";
  for (let i = 0; i < 5; i++)
    imgUrl += CHAR_LIST[getRandomInt(CHAR_LIST.length)];
  return imgUrl;
}

function createHtmlFile(data) {
  if (!fs.existsSync(ARCHIVE_LOCATION)) fs.mkdirSync(ARCHIVE_LOCATION);

  const filePath = path.join(ARCHIVE_LOCATION, uuid.v4() + ".html");

  fs.writeFileSync(filePath, data, "utf-8");

  console.log(`Created Html file: ${filePath}`);

  return filePath;
}

async function getRandomImageUrl() {
  const targetUrl = `https://prnt.sc/${generateScreenshotId()}`;
  const siteData = await axios(targetUrl, {
    method: "GET",
    headers: {
      "User-Agent": getRandomUserAgent(),
    },
  })
    .then((res) => res.data)
    .catch((err) => null);

  if (siteData) {
    const $ = cheerio.load(siteData);
    const imgTargetLink = $("#screenshot-image").attr("src");
    if (
      imgTargetLink &&
      imgTargetLink !==
        "//st.prntscr.com/2022/02/22/0717/img/0_173a7b_211be8ff.png"
    )
      return imgTargetLink;
    else return await getRandomImageUrl();
  } else {
    return await getRandomImageUrl();
  }
}

// Program
(async () => {
  let fileData = "";
  for (let i = 0; i < FETCH_IMAGE_COUNT; i++) {
    const imageUrl = await getRandomImageUrl();
    fileData += `<img src="${imageUrl}">\n\n`;
    console.log(`Fetched image: ${i + 1}/${FETCH_IMAGE_COUNT}`);
  }

  const filePath = createHtmlFile(fileData);

  open(filePath);
})();
