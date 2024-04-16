
# Create a self-signed certificate for localhost
# Any prompts can be left blank / skipped by pressing enter

mkdir "cert" -ErrorAction SilentlyContinue
cd "cert"
& 'C:\Program Files\Git\bin\bash.exe' -c "openssl genrsa -out localhost.key 2048"
& 'C:\Program Files\Git\bin\bash.exe' -c "openssl req -new -key localhost.key -out localhost.csr"
& 'C:\Program Files\Git\bin\bash.exe' -c "openssl x509 -req -in localhost.csr -signkey localhost.key -out localhost.crt"

cd ..