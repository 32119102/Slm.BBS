docker rm -f bbs-site
docker run -d --restart=always --name bbs-site -p 5000:5000 bbs-site:1.0.4
