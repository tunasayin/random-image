import random, cloudscraper, uuid, os, alive_progress, time
from bs4 import BeautifulSoup
from tools import *


filename = str(uuid.uuid1())

archive_location = os.path.join('.', 'archive')

def create_html_file():
    if not os.path.isdir(archive_location):
        os.mkdir(archive_location)
    for i in os.listdir('.'):
        if i.endswith(".html"):
            os.rename(i, f"{archive_location}/{i}")
            print(f'HTML FILE FOUND: {i} was moved to the archive folder')
    with open(filename + ".html", "w", encoding="utf-8") as file:
        file.write(
    """
    <html>
        <body>
    """
        )
    print(f"New Html file created: {filename}")


def insert_images(photo_count = 200):
    scraper = cloudscraper.create_scraper()
    with alive_progress.alive_bar(200, dual_line=True, title='Random-Image') as bar:
        for i in range(photo_count):
            
            headers = {"User-Agent": random.choice(agents)}
            param = ""
            for i in range(6):
                param += random.choice(char_list)
                
            html = scraper.get(f"https://prnt.sc/{param}", headers=headers).content

            soup = BeautifulSoup(html, features="html.parser")
            try: 
                imgcontent = soup.findAll("img")[0]["src"] 
            
            
                with open(filename + ".html", "a", encoding="utf-8") as file:
                    file.write(
        f"""
        <img style="max-width: 70vw" src={imgcontent}>
        """)
                bar()
                
            except IndexError: print('Agent banned :(')


def open_file_in_browser():
    os.startfile(os.path.join(os.path.abspath('.'), filename + '.html'))    
    print('Opening file...')
    
if __name__ == "__main__" :
    create_html_file()
    insert_images()
    open_file_in_browser()
    print(f"Done! Your file is ready at {os.path.abspath('.')}")