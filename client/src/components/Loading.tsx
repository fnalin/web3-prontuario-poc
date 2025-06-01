
type LoadingProps = {
    message?: string
    fullScreen?: boolean
}

export default function Loading({ message = 'Carregando...', fullScreen = false }: LoadingProps) {
    const content = (
        <div className="d-flex flex-column align-items-center justify-content-center text-secondary">
            <div className="spinner-border text-primary mb-3" role="status" style={{ width: '3rem', height: '3rem' }}>
                <span className="visually-hidden">Loading...</span>
            </div>
            <div>{message}</div>
        </div>
    )

    if (fullScreen) {
        return (
            <div
                className="position-fixed top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center bg-white bg-opacity-75"
                style={{ zIndex: 1050 }}
            >
                {content}
            </div>
        )
    }

    return content
}