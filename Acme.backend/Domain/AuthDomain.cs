using AutoMapper;
using Infraestructure.Helpers;
using Model.Common;
using Model.DataBase;
using Model.DTO.Auth;
using Model.DTO.Usuario;
using Repository.Interfaces;

namespace Domain
{
    public class AuthDomain
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthDomain(
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            JwtTokenGenerator jwtTokenGenerator)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Response<AuthResponse>> LoginAsync(LoginRequest request)
        {
            try
            {
                // Buscar usuario por email
                var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

                if (usuario == null)
                {
                    return Response<AuthResponse>.ErrorResponse(
                        "Credenciales inválidas",
                        new List<string> { "El email o contraseña son incorrectos" }
                    );
                }

                // Verificar contraseña
                if (!PasswordHasher.VerifyPassword(request.Password, usuario.password_hash))
                {
                    return Response<AuthResponse>.ErrorResponse(
                        "Credenciales inválidas",
                        new List<string> { "El email o contraseña son incorrectos" }
                    );
                }

                // Generar token JWT
                var token = _jwtTokenGenerator.GenerateToken(usuario);

                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

                var authResponse = new AuthResponse
                {
                    Token = token,
                    Usuario = usuarioDto
                };

                return Response<AuthResponse>.SuccessResponse(
                    authResponse,
                    "Login exitoso"
                );
            }
            catch (Exception ex)
            {
                return Response<AuthResponse>.ErrorResponse(
                    "Error al procesar login",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<AuthResponse>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Verificar si el email ya existe
                if (await _usuarioRepository.EmailExistsAsync(request.Email))
                {
                    return Response<AuthResponse>.ErrorResponse(
                        "Error en el registro",
                        new List<string> { "El email ya está registrado" }
                    );
                }

                // Crear nuevo usuario
                var usuario = _mapper.Map<Usuario>(request);
                usuario.password_hash = PasswordHasher.HashPassword(request.Password);
                usuario.estado = 1;
                usuario.creado_en = DateTime.UtcNow;

                await _usuarioRepository.CreateAsync(usuario);

                // Generar token JWT
                var token = _jwtTokenGenerator.GenerateToken(usuario);

                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

                var authResponse = new AuthResponse
                {
                    Token = token,
                    Usuario = usuarioDto
                };

                return Response<AuthResponse>.SuccessResponse(
                    authResponse,
                    "Usuario registrado exitosamente"
                );
            }
            catch (Exception ex)
            {
                return Response<AuthResponse>.ErrorResponse(
                    "Error al procesar registro",
                    new List<string> { ex.Message }
                );
            }
        }
    }
}
