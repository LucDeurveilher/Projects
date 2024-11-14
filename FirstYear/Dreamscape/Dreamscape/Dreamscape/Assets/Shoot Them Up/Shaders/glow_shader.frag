uniform float glow_intensity; // défini dans le vertex shader
uniform sampler2D texture;

void main()
{
    // Récupérer la couleur du pixel de la texture
    vec4 color = texture2D(texture, gl_TexCoord[0].xy);

    // Calculer la somme des couleurs environnantes
    vec4 sum = vec4(0.0);
    sum += texture2D(texture, gl_TexCoord[0].xy + vec2(-0.01, -0.01));
    sum += texture2D(texture, gl_TexCoord[0].xy + vec2(-0.01, 0.01));
    sum += texture2D(texture, gl_TexCoord[0].xy + vec2(0.01, -0.01));
    sum += texture2D(texture, gl_TexCoord[0].xy + vec2(0.01, 0.01));

    // Moyenne des couleurs environnantes
    vec4 average = sum / 4.0;

    // Calcul de la nouvelle couleur avec l'effet de lueur
    vec4 glow_color = color + (glow_intensity * (color - average));

    // Définition de la couleur finale du pixel
    gl_FragColor = glow_color;
}

